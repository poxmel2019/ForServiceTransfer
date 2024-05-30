// create folder

string service;
Console.WriteLine("Service:");
while (true)
{
    service = Console.ReadLine();

    if (string.IsNullOrEmpty(service))
    {
        Console.WriteLine("Empty service!");
    }
    else
    {
        break;
    }
}

Console.WriteLine("terminalNon3DId:");
string terminalNon3DId = Console.ReadLine();
Console.WriteLine("bin");
string bin = Console.ReadLine();

string address = "C:\\for_work\\db\\for_service_transfer\\";
string dir = $"{address}{service}";
Console.WriteLine(dir);

if (!Directory.Exists(dir))
{
    Directory.CreateDirectory(dir);
}

// create file transaction.sql
string transaction_text = "BEGIN TRY\r\n       " +
    " BEGIN TRANSACTION\r\n\t\t\r\n        " +
    "COMMIT TRANSACTION\r\n        " +
    "PRINT 'X rows inserted. Operation Successful'\r\nEND TRY\r\nBEGIN\r\n        " +
    "CATCH\r\n        " +
    "IF (@@TRANCOUNT > 0)\r\n                        " +
    "BEGIN\r\n                                        " +
    "ROLLBACK TRANSACTION;\r\n                                " +
    "PRINT 'Error inserting, all changes reversed'\r\n                        " +
    "END\r\nSELECT\r\n        " +
    "ERROR_NUMBER() AS ErrorNumber,\r\n        " +
    "ERROR_SEVERITY() AS ErrorSeverity,\r\n        " +
    "ERROR_STATE() AS ErrorState,\r\n        " +
    "ERROR_PROCEDURE() AS ErrorProcedure,\r\n        " +
    "ERROR_LINE() AS ErrorLine,\r\n        " +
    "ERROR_MESSAGE() AS ErrorMessage\r\n        " +
    "END CATCH\r\n;";


string transaction_file = $"{dir}\\transaction.sql";

// create file receipt.sql
string receipt_text = $"select * from service_templates " +
    $"\r\nwhere service_name = '{service}'\r\n;\r\n\r\ninsert into service_templates\r\n" +
    $"(service_name,portal_name,template_id)\r\nvalues\r\n('{service}','hb3halyk',21)\r\n;";

string receipt_file = $"{dir}\\receipt.sql";

// create file terminal.sql
string terminal_text =
    "-- Services\r\n" +
    "select * \r\n" +
    "from [services] \r\n" +                                               //81
    $"where [name] = '{service}'\r\n;\r\n\r\n" +

    "-- PortalServices\r\n" +
    "select s.name serv, p.name portal," +
    "\r\nps.* from PortalServices ps" +
    "\r\njoin [services] s" +
    "\r\non ps.ServiceId = s.Id\r\n" +
    "join Portals p\r\n" +
    "on ps.PortalId = p.Id\r\n" +
    $"where s.Name = '{service}'\r\n" +
    "order by p.name\r\n;\r\n\r\n" +

    "-- PortalServiceProperties\r\n" +
    "select \r\ns.name serv, p.name portal, " +
    "\r\nsp.name servprop, psp.*\r\n" +
    "from PortalServiceProperties psp\r\n" +
    "join PortalServices ps\r\non psp.PortalServiceId = ps.Id\r\n" +
    "join Portals p\r\non ps.PortalId = p.Id\r\n" +

    "join [Services] s\r\non ps.ServiceId = s.Id\r\n" +
    "join ServiceProperties sp\r\non psp.ServicePropertyId = sp.Id\r\n" +
    $"where s.name = '{service}'\r\n" +
    "order by p.name, sp.Name\r\n;\r\n\r\n" +

    "-- PortalServiceStaticPropertyValues\r\n" +
    "select \r\ns.name serv, p.name portal, \r\n" +
    "sp.name servprop, sp.ServicePropertyTypeId,\r\n" +
    "psp.PortalServicePropertyTypeId, psspv.*\r\n" +
    "from PortalServiceStaticPropertyValues psspv\r\n" +
    "join PortalServiceProperties psp\r\n" +
    "on psspv.PortalServicePropertyId = psp.Id\r\n" +
    "join PortalServices ps\r\non psp.PortalServiceId = ps.Id\r\n" +
    "join Portals p\r\non ps.PortalId = p.Id\r\n" +
    "join [Services] s\r\non ps.ServiceId = s.Id\r\n" +
    "join ServiceProperties sp\r\non psp.ServicePropertyId = sp.Id\r\n" +
    $"where s.name = '{service}'\r\n" +
    "order by p.name, sp.Name\r\n;\r\n\r\n" +

    "-- terminalNon3DId\r\n" +
    "select \r\ns.name serv, p.name portal, \r\nsp.name servprop, " +
    "sp.ServicePropertyTypeId,\r\npsp.PortalServicePropertyTypeId, \r\n" +
    "psspv.*\r\n" +
    $"-- update psspv set value = '{terminalNon3DId}'\r\n" +
    "from PortalServiceStaticPropertyValues psspv\r\n" +
    "join PortalServiceProperties psp\r\n" +
    "on psspv.PortalServicePropertyId = psp.Id\r\n" +
    "join PortalServices ps\r\n" +
    "on psp.PortalServiceId = ps.Id\r\n" +
    "join Portals p\r\non ps.PortalId = p.Id\r\n" +
    "join [Services] s\r\non ps.ServiceId = s.Id\r\n" +
    "join ServiceProperties sp\r\n" +
    "on psp.ServicePropertyId = sp.Id\r\n" +
    $"where s.name = '{service}'\r\n" +
    "and sp.name = 'terminalNon3DId'\r\n" +
    "-- '10f00b32-4751-4e00-883d-0d49d3dc5220'\r\n" +
    "order by p.name, sp.Name\r\n;" +
    "\r\n\r\n" +
    "/*" +
    "\r\n\r\n" +
    "*/"
    ;

string terminal_file = $"{dir}\\terminal.sql";

// create file bin.sql
string bin_text =                                                           //81 
    "-- PortalServices\r\n" +
    "select s.name serv, p.name portal," +
    "\r\nps.* " +
    "from PortalServices ps\r\n" +
    "join [services] s\r\n" +
    "on ps.ServiceId = s.Id\r\n" +
    "join Portals p\r\non ps.PortalId = p.Id\r\n" +
    $"where s.Name = '{service}'\r\n" +
    "order by p.name\r\n;" +
    "\r\n\r\n" +

    "-- bin\r\n" +
    "select * from ServiceProperties\r\n" +
    "where name = 'bin'\r\n-- 803\r\n\r\n" +

    "-- PortalServiceProperties\r\n" +
    "-- check\r\n" +
    "select \r\ns.name serv, p.name portal, " +
    "\r\nsp.name servprop, psp.*\r\n" +
    "from PortalServiceProperties psp\r\n" +
    "join PortalServices ps\r\n" +
    "on psp.PortalServiceId = ps.Id" +
    "\r\njoin Portals p\r\n" +
    "on ps.PortalId = p.Id\r\n" +
    "join [Services] s\r\n" +
    "on ps.ServiceId = s.Id\r\njoin ServiceProperties sp\r\n" +
    "on psp.ServicePropertyId = sp.Id\r\n" +                                //81
    $"where \r\ns.name = '{service}'\r\n" +
    "and sp.Name = 'bin'\r\n" +
    "order by p.name, sp.Name\r\n;\r\n" +
    "-- action\r\n" +
    "insert into PortalServiceProperties\r\n" +
    "(\r\nPortalServiceId,ServicePropertyId," +
    "PortalServicePropertyTypeId,ShowInDetails,\r\nHistoryPropertyId, " +
    "HoldUnmaskedValueForOutput, " +
    "\r\nValueCanBeUpdatedInProcessProperties\r\n)\r\n" +
    "values\r\n(,803,1,1,2,0,0),\r\n(,803,1,1,2,0,0)\r\n;\r\n\r\n" +

    "-- PortalServiceStaticPropertyValues\r\n" +
    "-- check\r\nselect \r\ns.name serv, p.name portal, " +
    "\r\nsp.name servprop, " +
    "sp.ServicePropertyTypeId,\r\npsp.PortalServicePropertyTypeId, " +
    "\r\npsspv.*\r\nfrom PortalServiceStaticPropertyValues psspv\r\n" +
    "join PortalServiceProperties psp\r\n" +
    "on psspv.PortalServicePropertyId = psp.Id\r\n" +
    "join PortalServices ps\r\n" +
    "on psp.PortalServiceId = ps.Id\r\n" +
    "join Portals p\r\non ps.PortalId = p.Id\r\n" +
    "join [Services] s\r\non ps.ServiceId = s.Id\r\n" +
    "join ServiceProperties sp\r\non psp.ServicePropertyId = sp.Id\r\n" +
    $"where \r\ns.name = '{service}'\r\n" +
    "and sp.Name = 'bin'\r\n" +
    "order by p.name, sp.Name\r\n;\r\n" +
    "-- action\r\n" +
    "insert into PortalServiceStaticPropertyValues\r\n" +
    "(PortalServicePropertyId,Value,IsVisible)\r\n" +
    $"values\r\n(,'{bin}',0),\r\n(,'{bin}',0)\r\n;\r\n\r\n\r\n"
    ;

string bin_file = $"{dir}\\bin.sql";


string[] texts = { bin_text, receipt_text, terminal_text, transaction_text };
string[] files = { bin_file, receipt_file, terminal_file, transaction_file };

var text_file = new Dictionary<string, string>();
for (int i = 0; i < texts.Length; i++)
{
    text_file.Add(files[i], texts[i]);
}

// by dictionary 
foreach (var elem in text_file)
{
    // by stream
    using (StreamWriter writer = new StreamWriter(elem.Key, false))
    {
        await writer.WriteLineAsync(elem.Value);
    }
}