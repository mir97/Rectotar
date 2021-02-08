using System.Linq;
using Rectotarat.Models;
using System;

// Инициализатор базы данных
namespace Rectotarat.Data
{
    public static class DbInitializer
    {
        public static void Initialize(RectoratContext context)
        {
            //string[] name = {"УВО1","УВО2","УВО3","УВО4", "УВО5", "УВО6", "УВО7" };
            //string[] ed = { "шт", "%", "Higth" };
            //string[] decription = { "Low", "Medium", "Higth" };


            context.Database.EnsureCreated();

            // Проверка занесены ли университеты
            if (context.Universitys.Any() & context.Achievements.Any() & context.Indicators.Any())
            {
                return;
            }


            context.AddRange(
                new University()
                {
                    UniversityName = "ГГТУ",
                    Address = "Пр-т Октября, 48, 246746, г. Гомель, Республика Беларусь",
                    Website = "gstu.by"
                },
                new University()
                {
                    UniversityName = "МИСТО",
                    Address = "Пр-т Октября, 58, 246746,246019, г. Гомель, Республика Беларусь",
                    Website = "mitso.by"
                },
                new University()
                {
                    UniversityName = "БГЭУКИП",
                    Address = "Пр-т Октября, 32,  246653, г. Гомель, Республика Беларусь",
                    Website = "i-bteu.by"
                },
                new University()
                {
                    UniversityName = "БЕЛГУТ",
                    Address = "Пр-т Октября, 48, 246746, г. Гомель, Республика Беларусь",
                                    Website = "gstu.by"
                },
                new University()
                {
                    UniversityName = "ГГМУ",
                    Address = "Пр-т Октября, 58, 246746,246019, г. Гомель, Республика Беларусь",
                    Website = "mitso.by"
                },
                new University()
                {
                    UniversityName = "ГГУ Скорыне",
                    Address = "Пр-т Октября, 32,  246653, г. Гомель, Республика Беларусь",
                    Website = "i-bteu.by"
                },
                new University()
                {
                  UniversityName = "ГГИ МЧС",
                  Address = "Пр-т Октября, 32,  246653, г. Гомель, Республика Беларусь",
                  Website = "i-bteu.by"
                }
            );


            context.SaveChanges();



            int year = 2018;

            /*1.1-1.17*/
            string[] NameindicatorRazdel1 = {
                "Доля педагогических работников из числа профессорско-преподавательского состава (в процентах от общего количества педагогических работников из числа профессорско-преподавательского состава):",
                "Количество педагогических работников, награжденных государственными наградами",
                "Наличие сертифицированной системы менеджмента качества",
                "Конкурс абитуриентов по итогам вступительной компании прошедшего года",
                "Средний проходной балл сертификатов абитуриентов по итогам вступительной компании прошедшего года ",
                "Доля обучающихся, получающих образование в дневной форме получения образования, ставших победителями (в процентах от общего количества обучающихся, получающих образование в дневной форме получения образования):",
                "Доля обучающихся, получающих образование в дневной форме получения образования (в процентах от общего количества обучающихся, получающих образование в дневной форме получения образования):",
                "Доля выпускников, получивших по окончанию высшего образования I ступени диплом о высшем образовании с отличием (в процентах от общего количества выпускников, получивших высшее образование I ступени)",
                "Отсутствие зафиксированных правоохранительными органами в отчётном году преступлений и правонарушений, совершённых:",
                "Доля выпускников, получивших распределение в прошедшем году (в процентах от общего количества выпускников прошедшего года)",
                "Доля обучающихся, получающих образование в дневной форме получения образования и участвующих в выполнении тем при бюджетном и внебюджетном финансировании (в процентах от  общего количества обучающихся, получающих образование в дневной форме получения образования)",
                "Позиция учреждения высшего образования в международных рейтингах:",
                "Количество монографий, изданных за отчетный год, (в расчете на одного педагогического работника  из числа профессорско-преподавательского состава)",
                "Количество выпущенных учебных изданий  (в расчете на одного штатного преподавателя из числа профессорско-преподавательского состава и научного работника) с грифом министерства образования Республики Беларусь",
                "Количество публикаций  (в расчете на одного штатного преподавателя  из числа профессорско-преподавательского состава и научного работника) за отчетный год:",
                "Количество патентов и авторских свидетельств, полученных за отчетный год (в расчете на одного штатного преподавателя  из числа профессорско-преподавательского  состава и научного работника)",
                "Доля аспирантов, докторантов окончивших аспирантуру, докторантуру с представлением диссертаций в специализированный совет (в процентах от общего количества аспирантов, докторантов окончивших аспирантуру, докторантуру в отчетном году)"
            };

            float[,] ValueindicatorRazdel1 = { 
                {0,0,0,0,0,0,0},/*1.1*/
                {1,2,2,0,0,0,0},/*1.2*/
                {1,1,1,1,1,1,0},/*1.3*/
                {(float)1.5,1,(float)1.87,(float)2.75,(float)1.43,(float)1.3,(float)1.21},/*1.4*/
                {194,185,277,181,182,156,152},/*1.5*/
                {0,0,0,0,0,0,0},/*1.6*/
                {0,0,0,0,0,0,0},/*1.7*/
                {(float)3.42,(float)6.51,(float)0.35,(float)0.9,(float)2.7,(float)4.8,14},/*1.8*/
                {0,0,0,0,0,0,0},/*1.9*/
                {(float)102.3,(float)102.2,(float)96.6,(float)102.8,(float)99.2,(float)89.1,0},/*1.10*/
                {(float)3.2,(float)6.11,0,1,(float)2.74,(float)2.4,0},/*1.11*/
                {0,0,0,0,0,0,0},/*1.12*/
                {(float)0.021,(float)0.61,(float)0.01,(float)0.015,(float)0.04,(float)0.024,0},/*1.13*/
                {0,0,0,0,0,0,0},/*1.14*/
                {(float)0.042,(float)0.061,(float)0.01,(float)0.02,(float)0.006,(float)0.0048,0},/*1/15*/
                {(float)0.014,(float)0.01,(float)0.02,(float)0.065,(float)0.021,(float)0.005,0},/*1.16*/
                {0,0,33,75,(float)44.4,0,0}/*1.17*/
            };


            int ID = 1;
            int temp = 1;


            for (ID = 1; ID < 18; ID++)/*Добавление подпунктов 2 уровня вложенности (раздела 1)*/
            {
                if (ID == 1 || ID == 14 || ID == 12 || ID == 9 || ID == 7 || ID == 6)/*Добавление для данных пунктов сортировки по min*/
                {
                    context.Indicators.Add(new Indicator { IndicatorId1 = 1, IndicatorId2 = Convert.ToByte(ID), IndicatorId3 = null, IndicatorName = NameindicatorRazdel1[ID - 1], IndicatorUnit = "", IndicatorType = IndicatorType.min, IndicatorDescription = null, Year = year });
                }
                else
                {

                    context.Indicators.Add(new Indicator { IndicatorId1 = 1, IndicatorId2 = Convert.ToByte(ID), IndicatorId3 = null, IndicatorName = NameindicatorRazdel1[ID - 1], IndicatorUnit = "", IndicatorType = IndicatorType.max, IndicatorDescription = null, Year = year });
                }
                
                context.SaveChanges();

                for (int i = 1; i < 8; i++)
                {
                    context.Achievements.Add(new Achievement { IndicatorId = ID, UnivercityId = i, IndicatorValue = ValueindicatorRazdel1[ID-1, i-1], Position = 22, Year = year });
                }
                context.SaveChanges();

                temp = temp + 1;
            }





            string[] NameindicatorRazdel2 = {
                "Объем экспорта услуг в сфере образования (тыс. руб.) в расчете на одного педагогического работника",
                "Объем привлеченных финансовых средств  на финансирование научной, научно-технической  и инновационной деятельности  (тыс. руб.)  в расчете на одного работника, участвующего  в выполнении  научно-исследовательских работ, опытно-конструкторских и технологических работ:",
                "Объем экспорта научной и научно-технической продукции (тыс. руб.) в расчете на одного работника, участвующего в выполнении научно-исследовательских работ, опытно-конструкторских и технологических работ",
            };


 

            float[,] ValueindicatorRazdel2 = {
                {(float)2.529,(float)3.81,(float)12.6,(float)1.67,(float)4.3,(float)3.7,(float)0.96},/*2.1*/
                {0,0,0,0,0,0,0},/*2.2*/
                {(float)9.35,0,0,(float)0.82,(float)0.05,0,0},/*2.3*/
            };



            for (ID = 1; ID < 4; ID++)/*Добавление подпунктов 2 уровня вложенности (раздела 2)*/
            {

                if (ID == 2)/*Добавление для данных пунктов сортировки по min*/
                {
                    context.Indicators.Add(new Indicator { IndicatorId1 = 2, IndicatorId2 = Convert.ToByte(ID), IndicatorId3 = null, IndicatorName = NameindicatorRazdel2[ID - 1], IndicatorUnit = "", IndicatorType = IndicatorType.min, IndicatorDescription = null, Year = year });
                }

                else
                {
                    context.Indicators.Add(new Indicator { IndicatorId1 = 2, IndicatorId2 = Convert.ToByte(ID), IndicatorId3 = null, IndicatorName = NameindicatorRazdel2[ID - 1], IndicatorUnit = "", IndicatorType = IndicatorType.max, IndicatorDescription = null, Year = year });
                }
                
                context.SaveChanges();

                for (int i = 1; i < 8; i++)
                {
                    context.Achievements.Add(new Achievement { IndicatorId = temp, UnivercityId = i, IndicatorValue = ValueindicatorRazdel2[ID - 1, i - 1], Position = 22, Year = year });
                }
                temp++;
                context.SaveChanges();
            }
            

   


            string[] NameindicatorRazdel3 = {
                "Объем внебюджетных средств, затраченных в отчетном году на обновление и развитие материально-технической базы (тыс. руб.) (в расчете на одного обучающегося, получающего высшее образование I и II ступени  в дневной форме получения образования)",
                "Доля компьютеров, подключенных к широкополосному доступу к глобальной компьютерной сети Интернет (в процентах от общего количества компьютеров, используемых в образовательном процессе)",
                "Выполнение показателей прогноза социально-экономического развития Республики Беларусь, утверждаемых Советом Министров Республики Беларусь:",
            };




            float[,] ValueindicatorRazdel3 = {
                {(float)0.26,(float)0.18,(float)0.5,(float)0.8,(float)0.02,(float)0.03,(float)0.56},/*3.1*/
                {100,100,(float)55.8,100,100,100,100},/*3.2*/
                {0,0,0,0,0,0,0},/*3.3*/
            };



            for (ID = 1; ID < 4; ID++)/*Добавление подпунктов 2 уровня вложенности (раздела 3)*/
            {


                if (ID == 3)/*Добавление для данных пунктов сортировки по min*/
                {
                    context.Indicators.Add(new Indicator { IndicatorId1 = 3, IndicatorId2 = Convert.ToByte(ID), IndicatorId3 = null, IndicatorName = NameindicatorRazdel3[ID - 1], IndicatorUnit = "", IndicatorType = IndicatorType.min, IndicatorDescription = null, Year = year });
                }

                else

                {
                    context.Indicators.Add(new Indicator { IndicatorId1 = 3, IndicatorId2 = Convert.ToByte(ID), IndicatorId3 = null, IndicatorName = NameindicatorRazdel3[ID - 1], IndicatorUnit = "", IndicatorType = IndicatorType.max, IndicatorDescription = null, Year = year });
                }

                context.SaveChanges();



                for (int i = 1; i < 8; i++)
                {
                    context.Achievements.Add(new Achievement { IndicatorId = temp, UnivercityId = i, IndicatorValue = ValueindicatorRazdel3[ID - 1, i - 1], Position = 22, Year = year });
                }
                temp++;
                context.SaveChanges();
            }



            string[] NameindicatorRazdeGlav = {
                "Доступность и качество образования",
                "Социально-экономическая деятельность",
                "Состояние материально-технической базы:",
            };


            for (ID = 1; ID < 4; ID++)/*Добавление подпунктов 1 уровня вложенности (разделов 1 2 3)*/
            {
                context.Indicators.Add(new Indicator { IndicatorId1 = Convert.ToByte(ID), IndicatorId2 = null, IndicatorId3 = null, IndicatorName = NameindicatorRazdeGlav[ID - 1], IndicatorUnit = "", IndicatorType = IndicatorType.min, IndicatorDescription = null, Year = year });
                context.SaveChanges();

                for (int i = 1; i < 8; i++)
                {
                    context.Achievements.Add(new Achievement { IndicatorId = temp, UnivercityId = i, IndicatorValue = 0, Position = 22, Year = year });
                }
                temp++;
                context.SaveChanges();
            }


            string[] NameindicatorPodRazdel = {
                "имеющих ученую степень, ученое звание",
                "в возрасте до 50 лет",
                "прошедших повышение квалификации (стажировку) за рубежом",
                "принявших участие в международных программах академической мобильности за рубежом",
                "международных (в том числе Союзного государства) олимпиад, спортивных соревнований, художественно-творческих мероприятий",
                "республиканских олимпиад, спортивных соревнований, художественно-творческих мероприятий",
                "республиканского конкурса научных работ студентов",
                "зачисленных в студенческие отряды",
                "получающих учебную стипендию",
                "обучающимися, получающими образование в дневной форме получения образования",
                "работниками",
                "QS World University Rankings",
                "Scimago Institutions Rankings",
                "Webometrics Ranking of  World Universities",
                "в научных журналах, включенных в перечень научных изданий Республики Беларусь для опубликования результатов диссертационных исследований ",
                "в журналах, входящих в базу данных SciVerse Scopus",
                "бюджетного финансирования",
                "внебюджетного финансирования",
                "валовый региональный продукт (к 2015 году)",
                "энергосбережение ",
                "средняя годовая заработная плата по отчетным данным, представляемым ежемесячно в Министерство за прошлый год",
            };

            float[,] ValueindicatorPodRazdel = {
                {(float)38.3,54,(float)42.8,(float)43.2,44,(float)45.9,50},/*1.1.1*/
                {63,(float)56.1,(float)73.5,60,62,(float)59.4,(float)62.5},/*1.2.1*/
                {(float)1.74,(float)9.6,(float)0.01,2,(float)3.56,(float)5.3,0},/*1.3.1*/
                {(float)3.73,(float)14.65,1,(float)2.13,(float)17.2,(float)0.48,0},/*1.4.1*/
                {(float)1.72,(float)0.21,0,(float)0.6,(float)0.56,(float)2.05,(float)2.3},/*1.6.1*/
                {(float)6.75,(float)4.24,(float)2.3,(float)2.1,(float)10.45,(float)2.6,0},/*1.6.2*/
                {(float)1.51,(float)1.76,0,2,(float)2.19,(float)0.6,(float)4.1},/*1.6.3*/
                {(float)58.2,(float)72.4,(float)37.9,59,(float)45.42,(float)16.3,(float)5.9},/*1.7.1*/
                {(float)91.7,(float)79.54,(float)56.67,63,(float)45.8,(float)68.9,(float)6.3},/*1.7.2*/
                {0,2,0,4,5,12,2},/*1.9.1*/
                {0,0,0,0,0,2,0},/*1.9.2*/
                {0,0,0,0,0,0,0},/*1.12.1*/
                {0,0,0,0,0,0,0},/*1.12.2*/
                {32,37,18,22,5,42,99},/*1.12.3*/
                {(float)0.312,(float)0.53,(float)0.47,(float)0.56,(float)0.44,(float)0.4,(float)0.65},/*1.14.1*/
                {(float)0.082,(float)0.005,0,(float)0.04,(float)0.052,(float)0.009,(float)0.07},/*1.14.2*/
                {(float)0.27,(float)0.12,(float)0.3,(float)1.65,(float)1.2,(float)1.49,0},/*2.2.1*/
                {(float)34.14,(float)0.177,0,(float)4.13,(float)1.09,(float)0.54,0},/*2.2.2*/
                {(float)83.3,(float)86.4,(float)102.2,(float)90.6,(float)96.7,87,0},/*3.3.1*/
                {(float)-3.8,(float)-3,(float)-3.4,(float)-4.6,(float)-3.3,-4,-5},/*3.3.2*/
                {(float)648.9,(float)739.7,(float)731,(float)500.3,(float)626.8,(float)519.2,(float)638.3},/*3.3.3*/

            };

            int[,] coords = {
            {1,1,1},
            {1,1,2},
            {1,1,3},
            {1,1,4},
            {1,6,1},
            {1,6,2},
            {1,6,3},
            {1,7,1},
            {1,7,2},
            {1,9,1},
            {1,9,2},
            {1,12,1},
            {1,12,2},
            {1,12,3},
            {1,14,1},
            {1,14,2},
            {2,2,1},
            {2,2,2},
            {3,3,1},
            {3,3,2},
            {3,3,3}
            };

            for (ID = 1; ID < 22; ID++)/*Добавление оставшихся подпунктов 3 уровня вложенности*/
            {

                if ((coords[ID - 1, 0] == 3 & coords[ID - 1, 1] == 3 & coords[ID - 1, 2] == 2 ) || ( (coords[ID - 1, 0] == 1 & coords[ID - 1, 1] == 9 & coords[ID - 1, 2] == 1) || (coords[ID - 1, 0] == 1 & coords[ID - 1, 1] == 9 & coords[ID - 1, 2] == 2) ) || (coords[ID - 1, 0] == 1 & coords[ID - 1, 1] == 12 & coords[ID - 1, 2] == 3) )
                {
                    context.Indicators.Add(new Indicator { IndicatorId1 = Convert.ToByte(coords[ID - 1, 0]), IndicatorId2 = Convert.ToByte(coords[ID - 1, 1]), IndicatorId3 = Convert.ToByte(coords[ID - 1, 2]), IndicatorName = NameindicatorPodRazdel[ID - 1], IndicatorUnit = "", IndicatorType = IndicatorType.min, IndicatorDescription = null, Year = year });
                }
                /*Добавление для данных пунктов сортировки по min*/



                else
                {
                    context.Indicators.Add(new Indicator { IndicatorId1 = Convert.ToByte(coords[ID - 1, 0]), IndicatorId2 = Convert.ToByte(coords[ID - 1, 1]), IndicatorId3 = Convert.ToByte(coords[ID - 1, 2]), IndicatorName = NameindicatorPodRazdel[ID - 1], IndicatorUnit = "", IndicatorType = IndicatorType.max, IndicatorDescription = null, Year = year });

                }

                context.SaveChanges();

                for (int i = 1; i < 8; i++)
                {

                    context.Achievements.Add(new Achievement { IndicatorId = temp, UnivercityId = i, IndicatorValue = ValueindicatorPodRazdel[ID - 1, i - 1], Position = 22, Year = year });

                }
                temp++;
                context.SaveChanges();
            }



        }

    }
}
