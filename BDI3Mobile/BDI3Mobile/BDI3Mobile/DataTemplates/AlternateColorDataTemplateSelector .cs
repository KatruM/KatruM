using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.DataTemplates
{
    public class AlternateColorDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EvenTemplate { get; set; }
        public DataTemplate UnevenTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        { 
            if(item is ChildAssessmentRecord)
            {
                return ((List<ChildAssessmentRecord>)((ListView)container).ItemsSource).IndexOf(item as ChildAssessmentRecord) % 2 == 0 ? EvenTemplate : UnevenTemplate;
            }
            else if(item is ChildRecord)
            {
                return ((List<ChildRecord>)((ListView)container).ItemsSource).IndexOf(item as ChildRecord) % 2 == 0 ? EvenTemplate : UnevenTemplate;
            }
            else if(item is ChildRecordsNewAssessment)
            {
                return ((List<ChildRecordsNewAssessment>)((ListView)container).ItemsSource).IndexOf(item as ChildRecordsNewAssessment) % 2 == 0 ? EvenTemplate : UnevenTemplate;
            }
            else if (item is ChildInformationRecord)
            {
                return ((List<ChildInformationRecord>)((ListView)container).ItemsSource).IndexOf(item as ChildInformationRecord) % 2 == 0 ? EvenTemplate : UnevenTemplate;
            }
            else
            {
                return null;
            }
        }
    }

    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }
    }


    public class AcademicDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemsInsScoreMatGrid { get; set; }
        public DataTemplate ScoreItemGrid { get; set; }
        public DataTemplate InsItemScoreGrid { get; set; }
        public DataTemplate InsImageItemScoreGrid { get; set; }
        public DataTemplate InsImageMatItemScoreGrid { get; set; }
        public DataTemplate ImageItemScoreGrid { get; set; }
        public DataTemplate ImageMatSampleGrid { get; set; }
        public DataTemplate ImageSampleGrid { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is AcademicTemplateSelectorModel)
            {
                var model = item as AcademicTemplateSelectorModel;
                if (model.CurrentAcademiTemplate == Models.AcademicFolder.CurrentAcademiTemplate.ImageItemScoreGrid)
                {
                    return ImageItemScoreGrid;
                }
                if (model.CurrentAcademiTemplate == Models.AcademicFolder.CurrentAcademiTemplate.ImageMaterialSampleGrid)
                {
                    return ImageMatSampleGrid;
                }
                if (model.CurrentAcademiTemplate == Models.AcademicFolder.CurrentAcademiTemplate.ImageSampleGrid)
                {
                    return ImageSampleGrid;
                }
                if (model.CurrentAcademiTemplate == Models.AcademicFolder.CurrentAcademiTemplate.InstructionImageMaterialItemScoreGrid)
                {
                    return InsImageMatItemScoreGrid;
                }
                if (model.CurrentAcademiTemplate == Models.AcademicFolder.CurrentAcademiTemplate.InstructionItemsScoreGrid)
                {
                    return InsItemScoreGrid;
                }
                if (model.CurrentAcademiTemplate == Models.AcademicFolder.CurrentAcademiTemplate.InstructionsImageItemsScoreGrid)
                {
                    return InsImageItemScoreGrid;
                }
                if (model.CurrentAcademiTemplate == Models.AcademicFolder.CurrentAcademiTemplate.ItemsInstructionScoreMaterialGrid)
                {
                    return ItemsInsScoreMatGrid;
                }
                if (model.CurrentAcademiTemplate == Models.AcademicFolder.CurrentAcademiTemplate.ScoreItemGrid)
                {
                    return ScoreItemGrid;
                }
            }
            return null;
        }
    }
}
