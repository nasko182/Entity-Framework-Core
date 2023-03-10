namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            //DbInitializer.ResetDatabase(context);

            //Test your solutions here

            Console.WriteLine(ExportSongsAboveDuration(context, 4));

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy",CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                        .Select(s=> new
                        {
                            s.Name,
                            s.Price,
                            Writer = s.Writer.Name
                        })
                        .OrderByDescending(s=>s.Name)
                        .ThenBy(s=>s.Writer)
                        .ToArray(),
                    a.Price
                })
                .ToArray()
                .OrderByDescending(a=>a.Price)
                .ToArray();

            foreach (var album in albums) 
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine("-Songs:");
                int counter = 1;
                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{counter++}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.Writer}");
                }
                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var sb = new StringBuilder();

            var songs = context.Songs
                .ToArray()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    PerformersFullNames = s.SongPerformers
                    .Select(sp =>  $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                    .OrderBy(sp => sp)
                    .ToArray(),
                    WriterName = s.Writer.Name,
                    ProducerName = s.Album!.Producer!.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s=>s.Name)
                .ThenBy(s=>s.WriterName)
                .ToArray();

            int counter = 1;
            foreach (var s in songs)
            {
                sb.AppendLine($"-Song #{counter++}");
                sb.AppendLine($"---SongName: {s.Name}");
                sb.AppendLine($"---Writer: {s.WriterName}");
                
                    foreach(var p in s.PerformersFullNames) 
                    {
                        sb.AppendLine($"---Performer: {p}");
                    }
                
                sb.AppendLine($"---AlbumProducer: {s.ProducerName}");
                sb.AppendLine($"---Duration: {s.Duration}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
