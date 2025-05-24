using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

public class DownloadController : Controller
{
    public IActionResult PdfWithoutRotativa()
    {
        var stream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(14));
                
                page.Header()
                    .Text("Data Sample PDF")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .Table(table =>
                    {
                        // Define columns
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100);
                            columns.RelativeColumn();
                        });

                        // Header row
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Nama");
                            header.Cell().Element(CellStyle).Text("Umur");
                        });

                        // Data rows
                        var data = new[]
                        {
                            new { Nama = "Andi", Umur = 25 },
                            new { Nama = "Budi", Umur = 30 },
                            new { Nama = "Cici", Umur = 22 }
                        };

                        foreach (var item in data)
                        {
                            table.Cell().Element(CellStyle).Text(item.Nama);
                            table.Cell().Element(CellStyle).Text(item.Umur.ToString());
                        }

                        // Cell styling function
                        static IContainer CellStyle(IContainer container)
                        {
                            return container
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(5);
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        }).GeneratePdf(stream);

        stream.Position = 0;
        return File(stream, "application/pdf", "DataSampleWithoutRotativa.pdf");
    }
}
