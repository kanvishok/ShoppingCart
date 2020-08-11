using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Awarious.Core.Infrastructure.Exceptions;

namespace Awarious.Core.Infrastructure
{
    public static class Utility
    {
        private static readonly long EpochMilliseconds =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000L;

        public static string BuildFolderPath(long key, int maxFiles = 10000, int depth = 2)
        {
            var parts = new List<string>();
            long current = key;

            for (int i = depth; i >= 0; i--)
            {
                long q = Convert.ToInt64(Math.Pow(maxFiles, i));
                long level = current / q;

                parts.Add($"{level:0000}");

                current = current % q;
            }

            //parts.Add(string.Format("{0:0000}{1}", current, extension));

            string separator = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
            string path = string.Join(separator, parts);

            return path;
        }

        /// <summary>
        /// Creates a sequential GUID according to SQL Server's ordering rules.
        /// </summary>
        public static Guid NewSequentialGuid()
        {
            // This code was not reviewed to guarantee uniqueness under most conditions, nor completely optimize for avoiding
            // page splits in SQL Server when doing inserts from multiple hosts, so do not re-use in production systems.
            var guidBytes = Guid.NewGuid().ToByteArray();

            // get the milliseconds since Jan 1 1970
            byte[] sequential =
                BitConverter.GetBytes((DateTime.Now.Ticks / 10000L) - EpochMilliseconds);

            // discard the 2 most significant bytes, as we only care about the milliseconds increasing, but the highest ones 
            // should be 0 for several thousand years to come (non-issue).
            if (BitConverter.IsLittleEndian)
            {
                guidBytes[10] = sequential[5];
                guidBytes[11] = sequential[4];
                guidBytes[12] = sequential[3];
                guidBytes[13] = sequential[2];
                guidBytes[14] = sequential[1];
                guidBytes[15] = sequential[0];
            }
            else
            {
                Buffer.BlockCopy(sequential, 2, guidBytes, 10, 6);
            }

            return new Guid(guidBytes);
        }

        public static Type FindTypeByName(string className, string[] assemblyNamesToSearch = null, string[] assemblyNamesToBeExcluded = null)
        {
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
	        if (assemblyNamesToSearch != null && assemblyNamesToSearch.Any())
	        {
		        assemblies = assemblies.Where(assembly => assemblyNamesToSearch.Contains(assembly.GetName().Name)).ToArray();
	        }

	        if (assemblyNamesToBeExcluded != null && assemblyNamesToBeExcluded.Any())
	        {
		        assemblies = assemblies.Where(assembly => !assemblyNamesToBeExcluded.Contains(assembly.FullName)).ToArray();
	        }

			foreach (Assembly a in assemblies)
			{
				var classType = a.GetTypes().FirstOrDefault(x => x.Name == className);
				if (classType == null)
				{
					continue;
				}

				return classType;
			}

	        return null;
        }

        public static string GetClassNameFromMessageStream(string message)
        {
	        ThrowException.IfNullOrWhiteSpace(message);

	        if (message.IndexOf("$type", StringComparison.Ordinal) == -1)
	        {
		        throw new TypeNotFoundException("Message Type cound not be identified from message stream");
	        }

	        var fields = message.Split(",");
	        if (fields != null && fields.Any())
	        {
		        var fieldWithClassName = fields.FirstOrDefault(x => x.Contains("$type"));
		        if (!string.IsNullOrWhiteSpace(fieldWithClassName))
		        {
			        var keyValue = fieldWithClassName.Split(":");
			        if (keyValue != null && keyValue.Length == 2)
			        {
				        var classNameSplit = keyValue[1].Split(".");
				        if (classNameSplit != null && classNameSplit.Length > 0)
				        {
					        return classNameSplit[classNameSplit.Length - 1];
				        }
			        }
		        }
	        }

	        throw new TypeNotFoundException("Message Type cound not be identified from message stream");
        }
	}
}
