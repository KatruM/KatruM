using BDI3Mobile.Models.DBModels;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectLocationPopupView : PopupPage
    {
        BasicReportViewModel ReportViewModel { get; set; }
        FullReportViewModel FullReportViewModel { get; set; }
        public SelectLocationPopupView(BasicReportViewModel basicReportViewModel= null, FullReportViewModel fullReportViewModel = null, List<string> SelectedLocations = null)
        {
            InitializeComponent();
            if (basicReportViewModel != null)
            {
                ReportViewModel = basicReportViewModel;
                BindingContext = basicReportViewModel;
                this.CloseWhenBackgroundIsClicked = true;
                LocationPopupView.Margin = new Thickness(0, 0, 0, 0);
                LocationPopupView.VerticalOptions = LayoutOptions.Center;
                LocationPopupView.HorizontalOptions = LayoutOptions.Center;
                this.BackgroundClicked += BasicReportPopupView_BackgroundClicked;
                ReportViewModel.SelectedLocations = new List<string>();
                if(SelectedLocations!=null)
                    ReportViewModel.SelectedLocations = SelectedLocations;
            }
            else if (fullReportViewModel != null)
            {
                FullReportViewModel = fullReportViewModel;
                BindingContext = fullReportViewModel;
                this.CloseWhenBackgroundIsClicked = true;
                this.BackgroundClicked += FullReportPopupView_BackgroundClicked;
                FullReportViewModel.SelectedLocations = new List<string>();
                if (SelectedLocations != null)
                    FullReportViewModel.SelectedLocations = SelectedLocations;
                LocationPopupView.Margin = new Thickness(0, 0, 0, 0);
                LocationPopupView.VerticalOptions = LayoutOptions.Center;
                LocationPopupView.HorizontalOptions = LayoutOptions.Center;

                if (Device.RuntimePlatform == Device.iOS)
                {
                    LocationPopupView.Margin = new Thickness(0, 10, 0, 0);
                }

            }
        }
        private void BasicReportPopupView_BackgroundClicked(object sender, EventArgs e)
        {
             ReportViewModel.isLocationPopupOpen = false;

            if (ReportViewModel.LocationsSelected == "Select location" || ReportViewModel.LocationsSelected == "No results found" || ReportViewModel.LocationsSelected == null)
                {
                    ReportViewModel.IsChildRecordButtonEnabled = false;
                    ReportViewModel.IsBatteryTypeButtonEnabled = false;
                    ReportViewModel.IsRecordFormButtonEnabled = false;

                    ReportViewModel.SelectedChild = null;
                    ReportViewModel.SelectedAssessmentType = null;
                    ReportViewModel.SelectedRecordForm = null;

                    ReportViewModel.OfflineStudentID = 0;
                    ReportViewModel.SelectedAssessmentID = 0;
                    ReportViewModel.SelectedRecordFormID = 0;
                   
                    ReportViewModel.RunReport = false;
                }
                else
                {
                    if (ReportViewModel.OfflineStudentID == 0)
                    {
                        ReportViewModel.SelectedChild = null;
                        ReportViewModel.SelectedAssessmentType = null;
                        ReportViewModel.SelectedRecordForm = null;
                        ReportViewModel.IsChildRecordButtonEnabled = true;
                        ReportViewModel.IsBatteryTypeButtonEnabled = false;
                        ReportViewModel.IsRecordFormButtonEnabled = false;
                    }
                    else
                    {
                        ReportViewModel.GetChildRecords();
                        ReportViewModel.SelectedAssessmentType = null;
                        ReportViewModel.SelectedRecordForm = null;
                        ReportViewModel.IsBatteryTypeButtonEnabled = false;
                        ReportViewModel.IsRecordFormButtonEnabled = false;
                }
            }           
        }
        private void FullReportPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            FullReportViewModel.isLocationPopupOpen = false;

            if (FullReportViewModel.LocationsSelected == null || FullReportViewModel.LocationsSelected == "Select location(s)")
            {
                FullReportViewModel.IsChildPopUpEnabled = false;
                FullReportViewModel.IsBatteryTypePopupEnabled = false;
                FullReportViewModel.IsRecordFormButtonEnabled = false;
                FullReportViewModel.SelectedChildID = null;
                FullReportViewModel.SelectedAssessmentID = 0;
                FullReportViewModel.SelectedRecordFormID = null;
                FullReportViewModel.SelectedScoreTypes = null;
                FullReportViewModel.IsScoresCheckboxCheckedChanged = FullReportViewModel.IsSDScoresCheckboxCheckedChanged = FullReportViewModel.IsActivitiesCheckboxCheckedChanged = FullReportViewModel.IsDomainCheckboxCheckedChanged = false;
                FullReportViewModel.IsScoringLayoutVisible = FullReportViewModel.IsStandardDeviationLayoutVisible = FullReportViewModel.IsBAESLayoutVisible = false;
                FullReportViewModel.RunReport = false;
                if (FullReportViewModel.SelectedNoteTypes != null && FullReportViewModel.SelectedNoteTypes.Count > 0)
                {
                    FullReportViewModel.SelectedNoteTypes.Clear();
                    FullReportViewModel.selectedNoteCount = 0;
                    FullReportViewModel.NoteSelected = "Select notes";
                }
                FullReportViewModel.RunReport = false;
            }
            else
            {
                    FullReportViewModel.SetChildData();
                    FullReportViewModel.IsChildPopUpEnabled = false;
                    FullReportViewModel.IsBatteryTypePopupEnabled = false;
                    FullReportViewModel.IsRecordFormButtonEnabled = false;
                    FullReportViewModel.SelectedChildID = null;
                    FullReportViewModel.SelectedAssessmentID = 0;
                    FullReportViewModel.SelectedRecordFormID = null;
                    FullReportViewModel.SelectedScoreTypes = null;
                    FullReportViewModel.IsScoresCheckboxCheckedChanged = FullReportViewModel.IsSDScoresCheckboxCheckedChanged = FullReportViewModel.IsActivitiesCheckboxCheckedChanged = FullReportViewModel.IsDomainCheckboxCheckedChanged = false;
                    FullReportViewModel.IsScoringLayoutVisible = FullReportViewModel.IsStandardDeviationLayoutVisible = FullReportViewModel.IsBAESLayoutVisible = false;
                    FullReportViewModel.RunReport = false;
                if (FullReportViewModel.SelectedNoteTypes != null && FullReportViewModel.SelectedNoteTypes.Count > 0)
                {
                    FullReportViewModel.SelectedNoteTypes.Clear();
                    FullReportViewModel.selectedNoteCount = 0;
                    FullReportViewModel.NoteSelected = "Select notes";
                }
                FullReportViewModel.RunReport = false;

            }
        }
        private void Level1ImageButton_Clicked(object sender, EventArgs e)
        {
            var clickedCell = (sender as ImageButton)?.BindingContext as LocationNew;
            var locations = ReportViewModel != null ? (this.BindingContext as BasicReportViewModel)?.LocationsObservableCollection : (this.BindingContext as FullReportViewModel)?.LocationsObservableCollection;
            var subLocations = ((sender as ImageButton)?.BindingContext as LocationNew)?.SubLocations.OrderByDescending(l => l.LocationName);

            if (ReportViewModel != null)
            {
                if (subLocations != null)
                {
                    foreach (var item in subLocations)
                    {
                        if (locations != null && !locations.Contains(item))
                        {
                            var index = locations.IndexOf(clickedCell) + 1;
                            if (clickedCell != null) clickedCell.IsExpanded = true;
                            (this.BindingContext as BasicReportViewModel)?.LocationsObservableCollection.Insert(index, item);
                        }
                        else
                        {
                            if (item.SubLocations != null)
                            {
                                foreach (var subItem in item.SubLocations)
                                {
                                    if (subItem.SubLocations != null)
                                    {
                                        foreach (var subItem2 in subItem.SubLocations)
                                        {
                                            if (subItem2.SubLocations != null)
                                            {
                                                foreach (var subItem3 in subItem2.SubLocations)
                                                {
                                                    (this.BindingContext as BasicReportViewModel)?.LocationsObservableCollection.Remove(subItem3);
                                                }
                                            }
                                            subItem2.IsExpanded = false;
                                            (this.BindingContext as BasicReportViewModel)?.LocationsObservableCollection.Remove(subItem2);
                                        }
                                    }
                                    subItem.IsExpanded = false;
                                    (this.BindingContext as BasicReportViewModel)?.LocationsObservableCollection.Remove(subItem);
                                }
                            }
                            item.IsExpanded = false;
                            if (clickedCell != null) clickedCell.IsExpanded = false;
                            (this.BindingContext as BasicReportViewModel)?.LocationsObservableCollection.Remove(item);

                        }
                    }
                }
            }
            else
            {
                if (subLocations != null)
                {
                    foreach (var item in subLocations)
                    {
                        if (locations != null && !locations.Contains(item))
                        {
                            var index = locations.IndexOf(clickedCell) + 1;
                            if (clickedCell != null) clickedCell.IsExpanded = true;
                            (this.BindingContext as FullReportViewModel)?.LocationsObservableCollection.Insert(index, item);
                        }
                        else
                        {
                            if (item.SubLocations != null)
                            {
                                foreach (var subItem in item.SubLocations)
                                {
                                    if (subItem.SubLocations != null)
                                    {
                                        foreach (var subItem2 in subItem.SubLocations)
                                        {
                                            if (subItem2.SubLocations != null)
                                            {
                                                foreach (var subItem3 in subItem2.SubLocations)
                                                {
                                                    (this.BindingContext as FullReportViewModel)?.LocationsObservableCollection.Remove(subItem3);
                                                }
                                            }
                                            subItem2.IsExpanded = false;
                                            (this.BindingContext as FullReportViewModel)?.LocationsObservableCollection.Remove(subItem2);
                                        }
                                    }
                                    subItem.IsExpanded = false;
                                    (this.BindingContext as FullReportViewModel)?.LocationsObservableCollection.Remove(subItem);
                                }
                            }
                            item.IsExpanded = false;
                            if (clickedCell != null) clickedCell.IsExpanded = false;
                            (this.BindingContext as FullReportViewModel)?.LocationsObservableCollection.Remove(item);

                        }
                    }
                }

            }
        }
        private void LocationListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ReportViewModel != null)
            {
                var count = ReportViewModel.selectedCount;
                var item = (LocationNew)e.SelectedItem;
                var locations = ReportViewModel != null ? (this.BindingContext as BasicReportViewModel)?.LocationsObservableCollection : (this.BindingContext as FullReportViewModel)?.LocationsObservableCollection;
                if (locations != null && locations.Contains(item))
                {
                    if (!item.IsEnabled)
                        return;
                    item.IsSelected = !item.IsSelected;
                    if (item.SubLocations != null)
                    {
                        foreach (var subitem in item.SubLocations)
                        {
                            subitem.IsSelected = item.IsSelected;
                            if (subitem.SubLocations != null)
                            {
                                foreach (var subitem2 in subitem.SubLocations)
                                {
                                    subitem2.IsSelected = item.IsSelected;
                                    if (subitem2.SubLocations != null)
                                    {
                                        foreach (var subitem3 in subitem2.SubLocations)
                                        {
                                            subitem3.IsSelected = item.IsSelected;
                                            if (subitem3.SubLocations != null)
                                            {
                                                foreach (var subitem4 in subitem3.SubLocations)
                                                {
                                                    subitem4.IsSelected = item.IsSelected;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (item.IsSelected)
                    {
                        if (!item.IsAddedToCount)
                        {
                            ReportViewModel.selectedCount = ReportViewModel.selectedCount + 1;
                            item.IsAddedToCount = true;
                        }
                        if (!(ReportViewModel.SelectedLocations.Contains(item.LocationName)))
                            ReportViewModel.SelectedLocations.Add(item.LocationName);

                        if (item.SubLocations != null)
                        {
                            foreach (var locs in item.SubLocations)
                            {
                                if (!locs.IsAddedToCount)
                                {
                                    ReportViewModel.selectedCount = ReportViewModel.selectedCount + 1;
                                    locs.IsAddedToCount = true;
                                }
                                ReportViewModel.LocationsSelected = ReportViewModel.selectedCount + " Selected";
                                if (!(ReportViewModel.SelectedLocations.Contains(locs.LocationName)))
                                    ReportViewModel.SelectedLocations.Add(locs.LocationName);

                                if (locs.SubLocations != null)
                                {
                                    foreach (var locs2 in locs.SubLocations)
                                    {
                                        if (!locs2.IsAddedToCount)
                                        {
                                            ReportViewModel.selectedCount = ReportViewModel.selectedCount + 1;
                                            locs2.IsAddedToCount = true;
                                        }
                                        ReportViewModel.LocationsSelected = ReportViewModel.selectedCount + " Selected";
                                        if (!(ReportViewModel.SelectedLocations.Contains(locs2.LocationName)))
                                            ReportViewModel.SelectedLocations.Add(locs2.LocationName);

                                        if (locs2.SubLocations != null)
                                        {
                                            foreach (var locs3 in locs2.SubLocations)
                                            {
                                                if (!locs3.IsAddedToCount)
                                                {
                                                    ReportViewModel.selectedCount = ReportViewModel.selectedCount + 1;
                                                    locs3.IsAddedToCount = true;
                                                }
                                                if (!(ReportViewModel.SelectedLocations.Contains(locs3.LocationName)))
                                                    ReportViewModel.SelectedLocations.Add(locs3.LocationName);

                                                ReportViewModel.LocationsSelected = ReportViewModel.selectedCount + " Selected";
                                                if (locs3.SubLocations != null)
                                                {
                                                    foreach (var locs4 in locs3.SubLocations)
                                                    {
                                                        if (!locs4.IsAddedToCount)
                                                        {
                                                            ReportViewModel.selectedCount = ReportViewModel.selectedCount + 1;
                                                            locs4.IsAddedToCount = true;
                                                        }
                                                        ReportViewModel.LocationsSelected = ReportViewModel.selectedCount + " Selected";
                                                        if (!(ReportViewModel.SelectedLocations.Contains(locs4.LocationName)))
                                                            ReportViewModel.SelectedLocations.Add(locs4.LocationName);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (ReportViewModel.selectedCount == 1)
                            {
                                ReportViewModel.LocationsSelected = item.LocationName;
                            }
                            else
                            {
                                ReportViewModel.LocationsSelected = ReportViewModel.selectedCount + " Selected";
                            }
                        }
                    }
                    else
                    {
                        if (item.SubLocations != null)
                        {
                            if (ReportViewModel.selectedCount > 0 && item.IsAddedToCount)
                            {
                                ReportViewModel.selectedCount = ReportViewModel.selectedCount - 1;
                            }
                            if (ReportViewModel.SelectedLocations.Contains(item.LocationName))
                                ReportViewModel.SelectedLocations.Remove(item.LocationName);
                            item.IsAddedToCount = false;
                            foreach (var locs in item.SubLocations)
                            {
                                if (ReportViewModel.selectedCount > 0 && locs.IsAddedToCount)
                                {
                                    ReportViewModel.selectedCount = ReportViewModel.selectedCount - 1;
                                }
                                locs.IsAddedToCount = false;

                                if (ReportViewModel.SelectedLocations.Contains(locs.LocationName))
                                    ReportViewModel.SelectedLocations.Remove(locs.LocationName);

                                ReportViewModel.LocationsSelected = ReportViewModel.selectedCount + " Selected";
                                if (locs.SubLocations != null)
                                {
                                    foreach (var locs2 in locs.SubLocations)
                                    {
                                        if (ReportViewModel.selectedCount > 0 && locs2.IsAddedToCount)
                                        {
                                            ReportViewModel.selectedCount = ReportViewModel.selectedCount - 1;
                                        }
                                        locs2.IsAddedToCount = false;
                                        if (ReportViewModel.SelectedLocations.Contains(locs2.LocationName))
                                            ReportViewModel.SelectedLocations.Remove(locs2.LocationName);

                                        ReportViewModel.LocationsSelected = ReportViewModel.selectedCount + " Selected";
                                        if (locs2.SubLocations != null)
                                        {
                                            foreach (var locs3 in locs2.SubLocations)
                                            {
                                                if (ReportViewModel.selectedCount > 0 && locs3.IsAddedToCount)
                                                {
                                                    ReportViewModel.selectedCount = ReportViewModel.selectedCount - 1;
                                                }
                                                locs3.IsAddedToCount = false;
                                                if (ReportViewModel.SelectedLocations.Contains(locs3.LocationName))
                                                    ReportViewModel.SelectedLocations.Remove(locs3.LocationName);
                                                ReportViewModel.LocationsSelected = ReportViewModel.selectedCount + " Selected";
                                                if (locs3.SubLocations != null)
                                                {
                                                    foreach (var locs4 in locs3.SubLocations)
                                                    {
                                                        if (ReportViewModel.selectedCount > 0)
                                                        {
                                                            ReportViewModel.selectedCount = ReportViewModel.selectedCount - 1;
                                                        }
                                                        locs4.IsAddedToCount = false;
                                                        if (ReportViewModel.SelectedLocations.Contains(locs4.LocationName))
                                                            ReportViewModel.SelectedLocations.Remove(locs4.LocationName);
                                                        ReportViewModel.LocationsSelected = ReportViewModel.selectedCount + " Selected";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ReportViewModel.selectedCount = ReportViewModel.selectedCount - 1;
                            item.IsAddedToCount = false;
                            if (ReportViewModel.SelectedLocations.Contains(item.LocationName))
                                ReportViewModel.SelectedLocations.Remove(item.LocationName);
                            ReportViewModel.LocationsSelected = ReportViewModel.selectedCount + " Selected";
                        }
                        if (ReportViewModel.selectedCount == 0)
                        {
                            ReportViewModel.LocationsSelected = "Select location";
                            ReportViewModel.SelectedLocations.Clear();
                            ReportViewModel.SelectedLocations = new List<string>();
                        }

                    }
                    ReportViewModel.SelectedLocationsReport = new List<string>();
                    ReportViewModel.SelectedLocationsReport = ReportViewModel.SelectedLocations;
                }

                if (ReportViewModel.selectedCount == 1)
                {
                    var itemSource = locationListview.ItemsSource.Cast<LocationNew>().ToList();
                    foreach (var items in itemSource.Where(items => items.IsSelected))
                    {
                        ReportViewModel.LocationsSelected = items.LocationName;
                    }
                }
               
                locationListview.SelectedItem = null;
            }
            else
            {
                var count = FullReportViewModel.selectedCount;
                var item = (LocationNew)e.SelectedItem;
                var locations = ReportViewModel != null ? (this.BindingContext as FullReportViewModel)?.LocationsObservableCollection : (this.BindingContext as FullReportViewModel)?.LocationsObservableCollection;
                if (locations != null && locations.Contains(item))
                {
                    if (!item.IsEnabled)
                        return;
                    item.IsSelected = !item.IsSelected;
                    if (item.SubLocations != null)
                    {
                        foreach (var subitem in item.SubLocations)
                        {
                            subitem.IsSelected = item.IsSelected;
                            if (subitem.SubLocations != null)
                            {
                                foreach (var subitem2 in subitem.SubLocations)
                                {
                                    subitem2.IsSelected = item.IsSelected;
                                    if (subitem2.SubLocations != null)
                                    {
                                        foreach (var subitem3 in subitem2.SubLocations)
                                        {
                                            subitem3.IsSelected = item.IsSelected;
                                            if (subitem3.SubLocations != null)
                                            {
                                                foreach (var subitem4 in subitem3.SubLocations)
                                                {
                                                    subitem4.IsSelected = item.IsSelected;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (item.IsSelected)
                    {
                        if (!item.IsAddedToCount)
                        {
                            FullReportViewModel.selectedCount = FullReportViewModel.selectedCount + 1;
                            item.IsAddedToCount = true;
                        }
                        if (!(FullReportViewModel.SelectedLocations.Contains(item.LocationId.ToString())))
                            FullReportViewModel.SelectedLocations.Add(item.LocationId.ToString());

                        if (item.SubLocations != null)
                        {
                            foreach (var locs in item.SubLocations)
                            {
                                if (!locs.IsAddedToCount)
                                {
                                    FullReportViewModel.selectedCount = FullReportViewModel.selectedCount + 1;
                                    locs.IsAddedToCount = true;
                                }
                                FullReportViewModel.LocationsSelected = FullReportViewModel.selectedCount + " Selected";
                                if (!(FullReportViewModel.SelectedLocations.Contains(locs.LocationId.ToString())))
                                    FullReportViewModel.SelectedLocations.Add(locs.LocationId.ToString());

                                if (locs.SubLocations != null)
                                {
                                    foreach (var locs2 in locs.SubLocations)
                                    {
                                        if (!locs2.IsAddedToCount)
                                        {
                                            FullReportViewModel.selectedCount = FullReportViewModel.selectedCount + 1;
                                            locs2.IsAddedToCount = true;
                                        }
                                        FullReportViewModel.LocationsSelected = FullReportViewModel.selectedCount + " Selected";
                                        if (!(FullReportViewModel.SelectedLocations.Contains(locs2.LocationId.ToString())))
                                            FullReportViewModel.SelectedLocations.Add(locs2.LocationId.ToString());

                                        if (locs2.SubLocations != null)
                                        {
                                            foreach (var locs3 in locs2.SubLocations)
                                            {
                                                if (!locs3.IsAddedToCount)
                                                {
                                                    FullReportViewModel.selectedCount = FullReportViewModel.selectedCount + 1;
                                                    locs3.IsAddedToCount = true;
                                                }
                                                if (!(FullReportViewModel.SelectedLocations.Contains(locs3.LocationId.ToString())))
                                                    FullReportViewModel.SelectedLocations.Add(locs3.LocationId.ToString());

                                                FullReportViewModel.LocationsSelected = FullReportViewModel.selectedCount + " Selected";
                                                if (locs3.SubLocations != null)
                                                {
                                                    foreach (var locs4 in locs3.SubLocations)
                                                    {
                                                        if (!locs4.IsAddedToCount)
                                                        {
                                                            FullReportViewModel.selectedCount = FullReportViewModel.selectedCount + 1;
                                                            locs4.IsAddedToCount = true;
                                                        }
                                                        FullReportViewModel.LocationsSelected = FullReportViewModel.selectedCount + " Selected";
                                                        if (!(FullReportViewModel.SelectedLocations.Contains(locs4.LocationId.ToString())))
                                                            FullReportViewModel.SelectedLocations.Add(locs4.LocationId.ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (FullReportViewModel.selectedCount == 1)
                            {
                                FullReportViewModel.LocationsSelected = item.LocationName;
                            }
                            else
                            {
                                FullReportViewModel.LocationsSelected = FullReportViewModel.selectedCount + " Selected";
                            }
                        }
                    }
                    else
                    {
                        if (item.SubLocations != null)
                        {
                            if (FullReportViewModel.selectedCount > 0 && item.IsAddedToCount)
                            {
                                FullReportViewModel.selectedCount = FullReportViewModel.selectedCount - 1;
                            }
                            if (FullReportViewModel.SelectedLocations.Contains(item.LocationId.ToString()))
                                FullReportViewModel.SelectedLocations.Remove(item.LocationId.ToString());
                            item.IsAddedToCount = false;
                            foreach (var locs in item.SubLocations)
                            {
                                if (FullReportViewModel.selectedCount > 0 && locs.IsAddedToCount)
                                {
                                    FullReportViewModel.selectedCount = FullReportViewModel.selectedCount - 1;
                                }
                                locs.IsAddedToCount = false;

                                if (FullReportViewModel.SelectedLocations.Contains(locs.LocationId.ToString()))
                                    FullReportViewModel.SelectedLocations.Remove(locs.LocationId.ToString());

                                FullReportViewModel.LocationsSelected = FullReportViewModel.selectedCount + " Selected";
                                if (locs.SubLocations != null)
                                {
                                    foreach (var locs2 in locs.SubLocations)
                                    {
                                        if (FullReportViewModel.selectedCount > 0 && locs2.IsAddedToCount)
                                        {
                                            FullReportViewModel.selectedCount = FullReportViewModel.selectedCount - 1;
                                        }
                                        locs2.IsAddedToCount = false;
                                        if (FullReportViewModel.SelectedLocations.Contains(locs2.LocationId.ToString()))
                                            FullReportViewModel.SelectedLocations.Remove(locs2.LocationId.ToString());

                                        FullReportViewModel.LocationsSelected = FullReportViewModel.selectedCount + " Selected";
                                        if (locs2.SubLocations != null)
                                        {
                                            foreach (var locs3 in locs2.SubLocations)
                                            {
                                                if (FullReportViewModel.selectedCount > 0 && locs3.IsAddedToCount)
                                                {
                                                    FullReportViewModel.selectedCount = FullReportViewModel.selectedCount - 1;
                                                }
                                                locs3.IsAddedToCount = false;
                                                if (FullReportViewModel.SelectedLocations.Contains(locs3.LocationId.ToString()))
                                                    FullReportViewModel.SelectedLocations.Remove(locs3.LocationId.ToString());
                                                FullReportViewModel.LocationsSelected = FullReportViewModel.selectedCount + " Selected";
                                                if (locs3.SubLocations != null)
                                                {
                                                    foreach (var locs4 in locs3.SubLocations)
                                                    {
                                                        if (FullReportViewModel.selectedCount > 0)
                                                        {
                                                            FullReportViewModel.selectedCount = FullReportViewModel.selectedCount - 1;
                                                        }
                                                        locs4.IsAddedToCount = false;
                                                        if (FullReportViewModel.SelectedLocations.Contains(locs4.LocationId.ToString()))
                                                            FullReportViewModel.SelectedLocations.Remove(locs4.LocationId.ToString());
                                                        FullReportViewModel.LocationsSelected = FullReportViewModel.selectedCount + " Selected";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            FullReportViewModel.selectedCount = FullReportViewModel.selectedCount - 1;
                            item.IsAddedToCount = false;
                            if (FullReportViewModel.SelectedLocations.Contains(item.LocationId.ToString()))
                                FullReportViewModel.SelectedLocations.Remove(item.LocationId.ToString());
                            FullReportViewModel.LocationsSelected = FullReportViewModel.selectedCount + " Selected";
                        }
                        if (FullReportViewModel.selectedCount == 0)
                        {
                            FullReportViewModel.LocationsSelected = "Select location(s)";
                            FullReportViewModel.SelectedLocations.Clear();
                            FullReportViewModel.SelectedLocations = new List<string>();
                        }

                        
                    }

                    FullReportViewModel.SelectedLocationsReport = new List<string>();
                    FullReportViewModel.SelectedLocationsReport = FullReportViewModel.SelectedLocations;
                }

                if (FullReportViewModel.selectedCount == 1)
                {
                    var itemSource = locationListview.ItemsSource.Cast<LocationNew>().ToList();
                    foreach (var items in itemSource.Where(items => items.IsSelected))
                    {
                        FullReportViewModel.LocationsSelected = items.LocationName;
                    }
                }

                locationListview.SelectedItem = null;
            }
        }       
    }
}