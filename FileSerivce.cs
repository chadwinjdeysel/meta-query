using System.Text;

public static class FileService {
    public static void WriteSchema(IEnumerable<Schema> schema) {
        var sb = new StringBuilder();

            sb.AppendLine("tables:");

            foreach(var table in schema)
            {
                sb.AppendLine($"{"  "}{table.TableName}:");

                if(!string.IsNullOrWhiteSpace(table.ColumnsAndTypes)) {
                    sb.AppendLine($"{"    "}columns:");

                    foreach(var column in table.ColumnsAndTypes.Split(','))
                    {
                        var columnAndType = column.Split('\\');
                        sb.AppendLine($"{"      "}{columnAndType[0]}: {columnAndType[1]}");
                    }
                }

                if(!string.IsNullOrWhiteSpace(table.Relationships)) {
                    sb.AppendLine($"{"    "}relationships:");

                    foreach(var relationship in table.Relationships.Split(','))
                    {
                        var relationshipAndColumn = relationship.Split('\\');
                        sb.AppendLine($"{"      "} - column: {relationshipAndColumn[0]}");
                        sb.AppendLine($"{"      "} - relationship: {relationshipAndColumn[1]}");
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter("schema.yaml"))
            {
                writer.Write(sb.ToString());
            }
    }
}