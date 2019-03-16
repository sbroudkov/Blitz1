using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Microsoft.VisualStudio.Tools.Applications;
using System.Windows.Forms;

namespace SetExcelDocumentProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            string assemblyLocation = "";
            Guid solutionID = new Guid();
            Uri deploymentManifestLocation = null;
            string documentLocation = "";
            string[] nonpublicCachedDataMembers = null;
            string strMessage = null;

            if (args.Count() != 4)
            {
                strMessage = "Командная строка:" + Environment.NewLine +
                    "SetExcelDocumentProperties.exe" + Environment.NewLine +
                    "   /assemblyLocation=<файл сборки>.dll" + Environment.NewLine +
                    "   /deploymentManifestLocation=<файл манифеста>.vsto" + Environment.NewLine +
                    "   /documentLocation=<файл документа>.xltx" + Environment.NewLine +
                    "   /solutionID=<идентификатор решения (GUID)>";
                MessageBox.Show(strMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            for (int i = 0; i <= args.Count() - 1; i++)
            {
                Console.WriteLine(args[i]);
                string[] oArugment = args[i].Split('=');

                switch (oArugment[0])
                {
                    case "/assemblyLocation":
                        assemblyLocation = oArugment[1];
                        break;
                    case "/deploymentManifestLocation":
                        deploymentManifestLocation = new Uri(oArugment[1], UriKind.Absolute);
                        break;
                    case "/documentLocation":
                        documentLocation = oArugment[1];
                        break;
                    case "/solutionID":
                        solutionID = Guid.Parse(oArugment[1]);
                        break;
                    default:
                        throw new Exception("Неизвестный ключ " + oArugment[0]);
                }
            }

            try
            {
                ServerDocument.RemoveCustomization(documentLocation);
                ServerDocument.AddCustomization(documentLocation, assemblyLocation,
                                            solutionID, deploymentManifestLocation,
                                            true, out nonpublicCachedDataMembers);
            }
            catch (System.IO.FileNotFoundException)
            {
                strMessage = "Указанный документ не найден.";
            }
            catch (System.IO.IOException)
            {
                strMessage = "Указанный документ не доступен для записи.";
            }
            catch (InvalidOperationException ex)
            {
                strMessage = "Невозможно удалить настройки уровня документа." + Environment.NewLine + ex.Message;
            }
            catch (DocumentNotCustomizedException ex)
            {
                strMessage = "Невозможно установить настройки уровня документа." + Environment.NewLine + ex.Message;
            }
            catch (Exception ex)
            {
                strMessage = "Ошибка." + Environment.NewLine + ex.Message;
            }
            finally
            {
                if (strMessage == null)
                {
                    MessageBox.Show(
                        "Установка свойств документа прошла успешно.", 
                        "Результат", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(strMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}