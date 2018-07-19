using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndexBuildCommon.Data
{
    public class FontSize
    {
        int flag;
        readonly int minSize = 95;
        readonly int defaultSize = 130;
        readonly int maxSize = 198;
        readonly int average;

        public FontSize(List<int> countList)
        {
            if (countList.Count == 0)
                return;
            average = countList.Sum() / countList.Count;
            if (countList.Count <= 4)
            {
                flag = average;
            }
            else
            {
                countList.Sort();
                int mid = countList.Count / 2;
                int count = 0;
                foreach (int i in countList)
                {
                    if (i < average)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                int k = mid - count;
                if (k >= 2)
                {
                    for (int i = 0; i < k; i++)
                    {
                        countList.RemoveAt(0);
                    }
                    flag = countList.Sum() / countList.Count;
                }
                else if (k <= -1)
                {
                    for (int i = 0; i < countList.Count - count; i++)
                    {
                        countList.RemoveAt(countList.Count - 1);
                    }
                }
                flag = countList.Sum() / countList.Count;
            }
        }

        public int GetFontSizePercent(int count)
        {
            if (count == 0)
            {
                return minSize;
            }
            if (count > 2 * average)
            {
                return maxSize;
            }

            int size = (int)(defaultSize * ((double)count / flag));

            if (size < minSize)
            {
                return minSize;
            }
            else if (size > 170)
            {
                return 170;
            }
            else
            {
                return size;
            }
        }
    }
}
