using Acr.UserDialogs;
using BDI3Mobile.IServices;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace BDI3Mobile.UWP.DependencyService
{
    public class CreateHtmlFiles : ICreateHtmlFiles
    {
        public async Task CreateHtmlFolders()
        {
            StorageFolder tempFolder = ApplicationData.Current.LocalFolder;
            var xmlfile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/es5/tex-mml-chtml.js"));
            var contentFolder = await tempFolder.CreateFolderAsync("contenthtml", CreationCollisionOption.OpenIfExists);
            var jsFolder = await contentFolder.CreateFolderAsync("es5", CreationCollisionOption.OpenIfExists);
            var file = await jsFolder.CreateFileAsync("tex-mml-chtml.js", CreationCollisionOption.ReplaceExisting);
            await xmlfile.CopyAndReplaceAsync(file);

            var stylesfile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/styles/record_forms.css"));
            var stylesFolder = await contentFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var stylesheetfile = await stylesFolder.CreateFileAsync("record_forms.css", CreationCollisionOption.ReplaceExisting);
            await stylesfile.CopyAndReplaceAsync(stylesheetfile);

            var assessmentStylesfile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/styles/assessment_form.css"));
            var assessmentStylesFolder = await contentFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var assessmentStylesheetfile = await assessmentStylesFolder.CreateFileAsync("assessment_form.css", CreationCollisionOption.ReplaceExisting);
            await assessmentStylesfile.CopyAndReplaceAsync(assessmentStylesheetfile);

            var itemFolderFolder = await tempFolder.CreateFolderAsync("ItemsFolder", CreationCollisionOption.OpenIfExists);
            var itemsjsFolder = await itemFolderFolder.CreateFolderAsync("es5", CreationCollisionOption.OpenIfExists);
            var itemjsfile = await itemsjsFolder.CreateFileAsync("tex-mml-chtml.js", CreationCollisionOption.ReplaceExisting);
            await xmlfile.CopyAndReplaceAsync(itemjsfile);

            var itemsstylesFolder = await itemFolderFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var itemstylesheetfile = await itemsstylesFolder.CreateFileAsync("record_forms.css", CreationCollisionOption.ReplaceExisting);
            await stylesfile.CopyAndReplaceAsync(itemstylesheetfile);

            var assessmentitemsstylesFolder = await itemFolderFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var assessmentitemstylesheetfile = await assessmentitemsstylesFolder.CreateFileAsync("assessment_form.css", CreationCollisionOption.ReplaceExisting);
            await assessmentStylesfile.CopyAndReplaceAsync(assessmentitemstylesheetfile);

            var scoringFolder = await tempFolder.CreateFolderAsync("ScoringFolder", CreationCollisionOption.OpenIfExists);
            var scoringjsFolder = await scoringFolder.CreateFolderAsync("es5", CreationCollisionOption.OpenIfExists);
            var scoringjsjsfile = await scoringjsFolder.CreateFileAsync("tex-mml-chtml.js", CreationCollisionOption.ReplaceExisting);
            await xmlfile.CopyAndReplaceAsync(scoringjsjsfile);

            var scoringstylesFolder = await scoringFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var scoringstylesheetfile = await scoringstylesFolder.CreateFileAsync("record_forms.css", CreationCollisionOption.ReplaceExisting);
            await stylesfile.CopyAndReplaceAsync(scoringstylesheetfile);

            var assessmentscoringstylesFolder = await scoringFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var assesmentscoringstylesheetfile = await assessmentscoringstylesFolder.CreateFileAsync("assessment_form.css", CreationCollisionOption.ReplaceExisting);
            await assessmentStylesfile.CopyAndReplaceAsync(assesmentscoringstylesheetfile);


            var ContentRubricFolder = await tempFolder.CreateFolderAsync("ContentRubric", CreationCollisionOption.OpenIfExists);
            var ContentRubricJSFolder = await ContentRubricFolder.CreateFolderAsync("es5", CreationCollisionOption.OpenIfExists);
            var ContentRubricJSfile = await ContentRubricJSFolder.CreateFileAsync("tex-mml-chtml.js", CreationCollisionOption.ReplaceExisting);
            await xmlfile.CopyAndReplaceAsync(ContentRubricJSfile);

            var ContentRubricstylesFolder = await ContentRubricFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var ContentRubricstylesheetfile = await ContentRubricstylesFolder.CreateFileAsync("record_forms.css", CreationCollisionOption.ReplaceExisting);
            await stylesfile.CopyAndReplaceAsync(ContentRubricstylesheetfile);

            var contentItemAttributeFolder = await tempFolder.CreateFolderAsync("ContentItemAttribute", CreationCollisionOption.OpenIfExists);
            var contentItemAttributejsFolder = await contentItemAttributeFolder.CreateFolderAsync("es5", CreationCollisionOption.OpenIfExists);
            var contentItemAttributejsfile = await contentItemAttributejsFolder.CreateFileAsync("tex-mml-chtml.js", CreationCollisionOption.ReplaceExisting);
            await xmlfile.CopyAndReplaceAsync(contentItemAttributejsfile);

            var contentItemAttributeStylesFolder = await contentItemAttributeFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var contentItemAttributeStylesSheeet = await contentItemAttributeStylesFolder.CreateFileAsync("record_forms.css", CreationCollisionOption.ReplaceExisting);
            await stylesfile.CopyAndReplaceAsync(contentItemAttributeStylesSheeet);

            var assessmentContentItemAttributeStylesFolder = await contentItemAttributeFolder.CreateFolderAsync("styles", CreationCollisionOption.OpenIfExists);
            var assessmentContentItemAttributeStylesSheeet = await assessmentContentItemAttributeStylesFolder.CreateFileAsync("assessment_form.css", CreationCollisionOption.ReplaceExisting);
            await assessmentStylesfile.CopyAndReplaceAsync(assessmentContentItemAttributeStylesSheeet);

            var materialContentfolder = await tempFolder.CreateFolderAsync("contenthtml", CreationCollisionOption.OpenIfExists);
            await materialContentfolder.CreateFileAsync("materialcontent.html", CreationCollisionOption.OpenIfExists);
            await materialContentfolder.CreateFileAsync("behaviourcontent.html", CreationCollisionOption.OpenIfExists);
            await materialContentfolder.CreateFileAsync("capturecontent.html", CreationCollisionOption.OpenIfExists);
            await materialContentfolder.CreateFileAsync("Descriptioncontent.html", CreationCollisionOption.OpenIfExists);
            await materialContentfolder.CreateFileAsync("ItemDescriptioncontent.html", CreationCollisionOption.OpenIfExists);
        }

        public async Task SaveFile(string filename)
        {
            try
            {
                StorageFolder tempFolder = ApplicationData.Current.LocalFolder;
                var reportStorageFile = await tempFolder.GetFileAsync(filename);
                var launchFile = await Windows.System.Launcher.LaunchFileAsync(reportStorageFile);
                if (launchFile)
                {
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync("Error while opening the Report.");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Error while opening the Report.");
            }
        }
    }
}
