using Experimental_Workshop.MVVM.Model;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Experimental_Workshop.ApplicationResourses
{
    public class PrintService
    {
        public static FlowDocument GenerateDocument(List<TechnologyCard> printCollection)
        {

            FlowDocument document = new FlowDocument();
            document.PageWidth = 793.76;
            document.PageHeight = 1122.56;
            document.FontSize = 22; 

            // tech card data
            foreach (var card in printCollection)
            {
                //if (card.IsInExpluatation == false)
                //    continue;
                //Creating new section of text
                Section section = new Section();
                section.BreakPageBefore = true;

                // Creating comapny name line
                Paragraph paragraph = new Paragraph();
                paragraph.TextAlignment = TextAlignment.Left;
                paragraph.Inlines.Add(new Run("LTD 'ExperimentalWorkshop'"));
                section.Blocks.Add(paragraph);

                //ApprovedLine
                paragraph = new Paragraph();
                paragraph.TextAlignment = TextAlignment.Center;
                paragraph.FontSize = 26;
                paragraph.Inlines.Add(new Bold(new Run("Approved:")));
                section.Blocks.Add(paragraph);

                //Director paragraph
                paragraph = new Paragraph();
                paragraph.TextAlignment = TextAlignment.Right;
                paragraph.FontSize = 26;
                paragraph.Margin = new Thickness(0,0,170,0);
                paragraph.Inlines.Add("Director:"); 
                section.Blocks.Add(paragraph);

                //if we have Director title

                if (card.Workers.Any(w=>w.JobTitle.Title == "Director" || w.JobTitle.Title == "director"))
                {
                    Worker director = card.Workers.First(w => w.JobTitle.Title == "Director" || w.JobTitle.Title == "director");
                    paragraph = new Paragraph();
                    paragraph.TextAlignment = TextAlignment.Right;
                    paragraph.FontSize = 26;
                    paragraph.Margin = new Thickness(0, 0, 20, 0);
                    paragraph.Inlines.Add($"{director.Name} {director.SecondName}");
                    section.Blocks.Add(paragraph);
                    
                }
                //if we havent Director Title
                else
                {
                        
                    paragraph = new Paragraph();
                    paragraph.TextAlignment = TextAlignment.Right;
                    Separator ubderlineDirector = new Separator();
                    ubderlineDirector.Width = 270;
                    ubderlineDirector.Height = 1;
                    paragraph.Inlines.Add(ubderlineDirector);
                    section.Blocks.Add(paragraph);
                }
                //TechCard name
                paragraph = new Paragraph();
                paragraph.TextAlignment = TextAlignment.Center;
                paragraph.FontSize = 28;
                paragraph.Inlines.Add(new Italic(new Run($"Technology Card: {card.Name}")));
                section.Blocks.Add(paragraph);

                // MicroprocessorType
                paragraph = new Paragraph();
                paragraph.TextAlignment = TextAlignment.Left;
                paragraph.Inlines.Add(new Bold(new Run ("Microprocessor  Type:"))); 
                paragraph.Inlines.Add($" {card.MicroprocessorType.Name}");
                section.Blocks.Add(paragraph);

                //Parametrs
                paragraph = new Paragraph();
                paragraph.TextAlignment = TextAlignment.Left;
                paragraph.Inlines.Add(new Bold(new Run("Parametrs:")));
                paragraph.Inlines.Add(new LineBreak());
                paragraph.Inlines.Add($"Frequency: {card.Frequency}");
                paragraph.Inlines.Add(new LineBreak());
                paragraph.Inlines.Add($"Power: {card.Power}");
                section.Blocks.Add(paragraph);

                //Materials
                paragraph = new Paragraph();
                paragraph.TextAlignment = TextAlignment.Left;
                paragraph.Inlines.Add(new Bold(new Run("Materials:")));
                section.Blocks.Add(paragraph);

                Table table = new Table();

                table.Columns.Add(new TableColumn { Width = new GridLength(5, GridUnitType.Star) }); 
                table.Columns.Add(new TableColumn { Width = new GridLength(3, GridUnitType.Star) }); 

                TableRowGroup headerRowGroup = new TableRowGroup();
                TableRow headerRow = new TableRow();

                headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Name"))));
                headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Amount"))));

                headerRowGroup.Rows.Add(headerRow);
                table.RowGroups.Add(headerRowGroup);

                if (card.Materials != null && card.Materials.Count > 0)
                {
                    foreach (var material in card.Materials)
                    {
                        TableRowGroup dataRowGroup = new TableRowGroup();
                        TableRow dataRow = new TableRow();

                        dataRow.Cells.Add(new TableCell(new Paragraph(new Run(material.Name))));
                        dataRow.Cells.Add(new TableCell(new Paragraph(new Run(material.Amount.ToString()))));
                        
                        dataRowGroup.Rows.Add(dataRow);
                        table.RowGroups.Add(dataRowGroup);
                    }
                }
                section.Blocks.Add(table);

                //Workers
                paragraph = new Paragraph();
                paragraph.TextAlignment = TextAlignment.Left;
                if (card.Workers != null && card.Workers.Count > 0)
                {
                    paragraph.Inlines.Add(new Bold(new Run("Workers:")));
                    foreach (var worker in card.Workers)
                    {
                        char symb = ',';
                        if (card.Workers.Last() == worker)
                            symb = '.';
                        paragraph.Inlines.Add($" {worker.Name} {worker.SecondName}"+symb);   
                    }
                }
                section.Blocks.Add(paragraph);

                //Deadline
                paragraph = new Paragraph();
                paragraph.TextAlignment = TextAlignment.Left;
                paragraph.Inlines.Add(new Bold(new Run($"Deadlines: ")));
                paragraph.Inlines.Add(new Run($"{card.DateOfStart.ToShortDateString()} - {card.DateOfEnd.ToShortDateString()}"));
                section.Blocks.Add(paragraph);

                //Today
                paragraph = new Paragraph();
                paragraph.TextAlignment= TextAlignment.Left;
                paragraph.Inlines.Add(new Bold(new Run("Date:   ")));
                Separator underlineDate = new Separator();
                underlineDate.Width = 100;
                underlineDate.Height = 1;
                paragraph.Inlines.Add(underlineDate);
                section.Blocks.Add(paragraph);

                //Signature
                Section signatureSection = new Section();
                paragraph = new Paragraph();
                paragraph.TextAlignment = TextAlignment.Right;
                Separator underlineSignature = new Separator();
                underlineSignature.Width = 170;
                underlineSignature.Height = 1;
                paragraph.Inlines.Add(underlineSignature);
                signatureSection.Blocks.Add(paragraph);
                paragraph= new Paragraph();
                paragraph.TextAlignment = TextAlignment.Right;
                paragraph.Margin = new Thickness(0,0,60,0);
                paragraph.FontSize = 10;
                paragraph.Inlines.Add(new Italic(new Run("(Signature)")));
                signatureSection.Blocks.Add(paragraph);
                section.Blocks.Add(signatureSection);

                //  Equipment
                //if (card.Equipment != null && card.Equipment.Count > 0)
                //{
                //    cardParagraph.Inlines.Add("Equipment:");
                //    cardParagraph.Inlines.Add(new LineBreak());
                //    foreach (var equipment in card.Equipment)
                //    {
                //        cardParagraph.Inlines.Add($"- {equipment.Name}");
                //        cardParagraph.Inlines.Add(new LineBreak());
                //    }
                //}
                
                document.Blocks.Add(section);
                
            }
            return document;
        }
        public static void PrintData(FlowDocument document)
        {
            
            PrintDialog dialog = new PrintDialog();
            dialog.ShowDialog();
            document.Foreground = Brushes.Black;
            
            dialog.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, "Printing Data");
        }
    }
}
