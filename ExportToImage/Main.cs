using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExportToImage
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;


            string desktop_path = Environment.GetFolderPath(                //ссылка на рабочий стол
              Environment.SpecialFolder.Desktop);

            List<ViewPlan> viewPlanList = new FilteredElementCollector(doc)  //список всех планов
                                    .OfClass(typeof(ViewPlan))
                                    .WhereElementIsNotElementType()
                                    .Cast<ViewPlan>()
                                    .ToList();

            var viewPlan = viewPlanList[0];                                 //План 1го этажа, 1й из списка

            string filepath = Path.Combine(desktop_path,                    //путь экспорта и имя этажа
              viewPlan.Name);

                                      

            IList<ElementId> imageExportList = new List<ElementId>();       //лист для экспорта
            imageExportList.Add(viewPlan.Id);

            ImageExportOptions img = new ImageExportOptions();
            
            //параметры экспорта___________________________
            img.ZoomType = ZoomFitType.FitToPage;
            img.PixelSize = 1256;
            img.ImageResolution = ImageResolution.DPI_600;
            img.FitDirection = FitDirectionType.Horizontal;
            img.ExportRange = ExportRange.SetOfViews;
            img.HLRandWFViewsFileType = ImageFileType.PNG;
            img.FilePath = filepath;
            img.ShadowViewsFileType = ImageFileType.PNG;
            //______________________________________________

            img.SetViewsAndSheets(imageExportList);
            doc.ExportImage(img);                                           //экспорт в изображение

            TaskDialog.Show("Выполнено", $"План 1го этажа экспортирован в изображение.{Environment.NewLine}");

            return Result.Succeeded;
        }
    }
}

