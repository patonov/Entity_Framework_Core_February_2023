namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            Console.WriteLine(ExportAlbumsInfo(context, 9));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums.Where(a => a.ProducerId == producerId)
                 .Select(a => new
                 {
                     a.Name,
                     ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                     ProducerName = a.Producer.Name,
                     songs = a.Songs.OrderByDescending(s => s.Name).ThenBy(s => s.Writer.Name)
                        .Select(s => new {
                            s.Name,
                            s.Price,
                            WriterName = s.Writer.Name
                        }).ToList(),
                     a.Price
                 }).OrderByDescending(a => a.Price).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var a in albums)
            {
                sb.AppendLine($"-AlbumName: {a.Name}");
                sb.AppendLine($"-ReleaseDate: {a.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {a.ProducerName}");
                sb.AppendLine("-Songs:");

                int i = 1;

                foreach (var s in a.songs)
                {                    
                    sb.AppendLine($"---#{i}");
                    sb.AppendLine($"---SongName: {s.Name}");
                    sb.AppendLine($"---Price: {s.Price:F2}");
                    sb.AppendLine($"---Writer: {s.WriterName}");

                    i++;
                }

                sb.AppendLine($"-AlbumPrice: {a.Price:F2}");
            }
            return sb.ToString().Trim();

        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
