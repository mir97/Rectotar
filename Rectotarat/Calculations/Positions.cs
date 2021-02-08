using System;
using System.Linq;
using System.Collections.Generic;
using Rectotarat.Models;
using Rectotarat.Calculations;

namespace GomelRectorCouncil.Calculations
{
    public class Positions
    {

        private static void arraySort(Element[] arrayInput, int SortParam)
        {

            if (SortParam == 0)
            {
                Array.Sort(arrayInput, new ValueComparer());
            }
            else
            {
                Array.Sort(arrayInput, new ReverseValueComparer());
            }
        }
        /**
         * check array for duplicates and change results to places
         *
         * @param array
         *            double
         * @param param
         *            int
         */
        public static float[] Calculate(float[] arrayValues, int SortParam)
        {

            // an Objects array
            Element[] input = new Element[arrayValues.Length];

            Element[] output = new Element[arrayValues.Length];

            for (int i = 0; i < arrayValues.Length; i++)
            {
                input[i] = new Element()
                {
                    Index = i,
                    Value = arrayValues[i]
                };
                output[i] = new Element()
                {
                    Index = i,
                    Value = arrayValues[i]
                };

            }

            // sort
            Positions.arraySort(input, SortParam);

            // used elements
            HashSet<float> used = new HashSet<float>();
            // output array
            float[] places = new float[arrayValues.Length];
            // index in array
            int place = 0;
            // count how many duplicates
            int count = 0;
            // calculated index
            float index;
            // going through array first cycle
            for (int i = 0; i < input.Length; i++)
            {
                // if we used this elements - go to next element
                if (used.Contains(input[i].Value))
                {
                    continue;
                }
                else
                {
                    // else - add element to collection
                    used.Add(input[i].Value);
                }

                // temporal list for indexes
                List<int> positions = new List<int>();
                // add current position
                positions.Add(i);
                for (int j = i + 1; j < input.Length; j++)
                {
                    // check if there are duplicate elements
                    if (input[i].CompareTo(input[j]) == 0)
                    {
                        // if yes - add to the list
                        positions.Add(j);
                    }
                }
                // index in array
                place = 0;
                // count how many duplicates
                count = 0;
                // calculated index
                // for each positions count the sum of indexes and count duplicates
                foreach (int p in positions)
                {
                    place += p;
                    count++;

                }
                // count result
                index = (float)place / count;
                // write them to output array
                foreach (int p in positions)
                {
                    output[p].Value = index + 1;
                }

            }

            for (int i = 0; i < input.Length; i++)
            {
                output[i].Index = input[i].Index;
            }
  

            // sort objects by the index to the previous view
            Array.Sort(output, new IndexComparer());

            for (int i = 0; i < places.Length; i++)
            {
                places[i] = output[i].Value;
            }
            return places;


        }
        public static List<Achievement> Get(List<Achievement> inputAchievementsYear)
        {
            List<Achievement> outputAchievementsYear=new List<Achievement>(inputAchievementsYear);
            int jj=0;
            //вычисление мест по уровню 3 для всех университетов на основе значений показателей
            var groupIndicatorCode3=inputAchievementsYear
                .Where(j=>(j.Indicator.IndicatorId3!=null))
                .GroupBy(j=>new {j.Indicator.IndicatorId1,j.Indicator.IndicatorId2,j.Indicator.IndicatorId3});
            foreach (var item in groupIndicatorCode3)
            {
                float[] inputarray=item.Select(t=>t.IndicatorValue).ToArray();
                int indicatorType=(int)item.Select(o=>o.Indicator.IndicatorType).FirstOrDefault();
                float[] outputarray=Calculate(inputarray,indicatorType);
                jj=0;
                foreach (var t in item)
                {
                    outputAchievementsYear.First(s=>s.AchievementId==t.AchievementId).Position=outputarray[jj];
                    jj+=1;
                }
            }

            //вычисление показателей по уровню 2 для каждого университетов на основе суммы значений мест по уровню 3
            var groupSumIndicatorCode2University=inputAchievementsYear
                .Where(j=>(j.Indicator.IndicatorId2!=null))
                .GroupBy(j=>new {j.Indicator.IndicatorId1,j.Indicator.IndicatorId2,j.UnivercityId});
            foreach (var item in groupSumIndicatorCode2University)
            {
                var subItems=item.Where(j=>(j.Indicator.IndicatorId3!=null));
                if (subItems.Count()>0)
                {
                    float sumElements=subItems.Sum(s=>s.Position);
                    var headElement=item.Where(j=>(j.Indicator.IndicatorId3==null)).FirstOrDefault();
                    if (headElement!=null)
                    {
                        outputAchievementsYear.First(s=>s.AchievementId==headElement.AchievementId).IndicatorValue=sumElements;
                    }
                }
            }
            //вычисление мест по уровню 2 для всех университетов на основе значений показателей
            var groupIndicatorCode2=inputAchievementsYear
                .Where(j=>(j.Indicator.IndicatorId2!=null&j.Indicator.IndicatorId3==null))
                .GroupBy(j=>new {j.Indicator.IndicatorId1,j.Indicator.IndicatorId2});
            foreach (var item in groupIndicatorCode2)
            {
                float[] inputarray=item.Select(t=>t.IndicatorValue).ToArray();
                int indicatorType=(int)item.Select(o=>o.Indicator.IndicatorType).FirstOrDefault();
                float[] outputarray=Calculate(inputarray,indicatorType);
                jj=0;
                foreach (var t in item)
                {
                    outputAchievementsYear.First(s=>s.AchievementId==t.AchievementId).Position=outputarray[jj];
                    jj+=1;
                }
            }

            //вычисление показателей по уровню 1 для каждого университетов на основе суммы значений мест по уровню 2
            var groupSumIndicatorCode1University=inputAchievementsYear
                .Where(j=>(j.Indicator.IndicatorId3==null))
                .GroupBy(j=>new {j.Indicator.IndicatorId1,j.UnivercityId});
            foreach (var item in groupSumIndicatorCode1University)
            {
                var subItems=item.Where(j=>(j.Indicator.IndicatorId2!=null));
                if (subItems.Count()>0)
                {
                    float sumElements=subItems.Sum(s=>s.Position);
                    var headElement=item.Where(j=>(j.Indicator.IndicatorId2==null)).FirstOrDefault();
                    if (headElement!=null)
                    {
                        outputAchievementsYear.First(s=>s.AchievementId==headElement.AchievementId).IndicatorValue=sumElements;
                    }
                }
            }

            //вычисление мест по уровню 1 для всех университетов на основе значений показателей
            var groupIndicatorCode1=inputAchievementsYear
                .Where(j=>(j.Indicator.IndicatorId2==null&j.Indicator.IndicatorId3==null))
                .GroupBy(j=>new {j.Indicator.IndicatorId1});
            foreach (var item in groupIndicatorCode1)
            {
                float[] inputarray=item.Select(t=>t.IndicatorValue).ToArray();
                int indicatorType=(int)item.Select(o=>o.Indicator.IndicatorType).FirstOrDefault();
                float[] outputarray=Calculate(inputarray,indicatorType);
                jj=0;
                foreach (var t in item)
                {
                    outputAchievementsYear.First(s=>s.AchievementId==t.AchievementId).Position=outputarray[jj];
                    jj+=1;
                }
            }         




            return outputAchievementsYear;

        }
    }
}















    




