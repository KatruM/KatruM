using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace BDI3Mobile.Models.BL
{
    public class ResetScore : BindableObject
    {
        public int SubDomainContentCatgoryId { get; set; }
        public string DomainName { get; set; } //Eg: Adaptive, Social-Emotional, Communication etc.,
        public string DomainCode { get; set; } //Eg: ADP, COM etc.,
        public string SubDomainName { get; set; } //Eg: "Self-Care", "Personal Responsibility" etc.,
        public string SubDomainCode{ get; set; } //Eg: SC, PR.,
        public bool IsSubDomain { get; set; } // If this is true; Domain Name will be filled with its ParentCategory
        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        //Area related properties - Academic Survey Litercay Form Scores
        private bool _isAreavisible;
        public bool IsAreaListVisible
        {
            get
            {
                return _isAreavisible;
            }
            set
            {
                _isAreavisible = value;
                OnPropertyChanged(nameof(IsAreaListVisible));
            }
        }

        int _height;
        public int Height

        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private List<ResetScore> _resetScores;
        public List<ResetScore> ResetScoresAreaList
        {
            get
            {
                return _resetScores;
            }
            set
            {
                _resetScores = value;
                OnPropertyChanged(nameof(ResetScoresAreaList));
                if(_resetScores != null && _resetScores.Count > 0)
                    Height = _resetScores.Count * 41;
            }
        }
    }
    /// <summary>
    /// Required to Show Grouping (CollectionView)
    /// </summary>
    public class ResetScoreGroup : List<ResetScore>
    {
        public string Name { get; private set; }
        
        public ResetScoreGroup(string name, List<ResetScore> resetScores) : base(resetScores)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
