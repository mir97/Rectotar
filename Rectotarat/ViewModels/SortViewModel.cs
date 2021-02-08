using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rectotarat.ViewModels
{
    public enum SortState
    {
        IndicatorCodeAsc,    // по коду показателя по возрастанию
        IndicatorCodeDesc,   // по коду показателя по убыванию
        UniversityNameAsc, // по университету по возрастанию
        UniversityNameDesc,    // по университету по убыванию
    }
    public class SortViewModel
    {
        public SortState IndicatorCodeSort { get; private set; } // значение для сортировки по коду показателя
        public SortState UniversityNameSort { get; private set; }    // значение для сортировки по университету
        public SortState Current { get; private set; }     // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
            IndicatorCodeSort = sortOrder == SortState.IndicatorCodeAsc ? SortState.IndicatorCodeDesc : SortState.IndicatorCodeAsc;
            UniversityNameSort = sortOrder == SortState.UniversityNameAsc ? SortState.UniversityNameDesc : SortState.UniversityNameAsc;
            Current = sortOrder;
        }
    }
}
