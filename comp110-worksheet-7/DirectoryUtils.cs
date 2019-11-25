using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp110_worksheet_7
{
	public static class DirectoryUtils
	{
		public static long GetFileSize(string filePath)
		{
			return new FileInfo(filePath).Length;
		}

		public static bool IsDirectory(string path)
		{
			return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
		}

		public static long GetTotalSize(string directory)
		{
			long count = 0;
			foreach (string file in Directory.GetFiles(directory))
				count += GetFileSize(file);

			foreach (string subdir in Directory.GetDirectories(directory))
				count += GetTotalSize(subdir);
			return count;
		}

		public static int CountFiles(string directory)
		{
			int count = Directory.GetFiles(directory).Length;
			foreach (string subdir in Directory.GetDirectories(directory))
				count += CountFiles(subdir);
			return count;
		}

		public static int GetDepth(string directory)
		{
			int maxDepth = 0;
			foreach (string subdir in Directory.GetDirectories(directory))
			{
				int depth = GetDepth(subdir);
				maxDepth = Math.Max(depth + 1, maxDepth);
			}
			return maxDepth;
		}

		public static Tuple<string, long> GetSmallestFile(string directory)
		{
			Tuple<string, long> best = new Tuple<string, long>("", long.MaxValue);
			foreach (string sub in Directory.GetFileSystemEntries(directory))
			{
				Tuple<string, long> candidate;
				if (IsDirectory(sub))
				{
					candidate = GetSmallestFile(sub);
				}
				else
				{
					candidate = new Tuple<string, long>(sub, GetFileSize(sub));
				}

				if (candidate.Item2 < best.Item2)
					best = candidate;
			}

			return best;
		}

		public static Tuple<string, long> GetLargestFile(string directory)
		{
			Tuple<string, long> best = new Tuple<string, long>("", long.MinValue);
			foreach (string sub in Directory.GetFileSystemEntries(directory))
			{
				Tuple<string, long> candidate;
				if (IsDirectory(sub))
				{
					candidate = GetLargestFile(sub);
				}
				else
				{
					candidate = new Tuple<string, long>(sub, GetFileSize(sub));
				}

				if (candidate.Item2 > best.Item2)
					best = candidate;
			}

			return best;
		}

		public static IEnumerable<string> GetFilesOfSize(string directory, long size)
		{
			foreach (string sub in Directory.GetFileSystemEntries(directory))
			{
				if (IsDirectory(sub))
				{
					foreach (var x in GetFilesOfSize(sub, size))
						yield return x;
				}
				else
				{
					if (GetFileSize(sub) == size)
						yield return sub;
				}
			}
		}
	}
}
