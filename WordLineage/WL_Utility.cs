using System;
using System.Collections.Generic;
using  System.Text;

namespace WordLineage
{
    public static class WL_Utility
    {
        public static string FormatStringListForDisplay(List<string> list,
            bool listIsNumbered, string spacer = " ", string prepend = "", string append = "")
        {
            if (list.Count <= 0)
            {   // return empty string if nothing in list
                return "";
            }
            else
            {
                StringBuilder sb = new StringBuilder("");

                for (int i = 0; i < list.Count; i++)
                {
                    if (listIsNumbered)
                    { // add a number and a space if list is Numbered
                        sb.Append((i + 1) + ". ");
                    }

                    sb.Append(prepend + list[i] + append);

                    // add the spacer unless it's the last item
                    if (i < list.Count - 1)
                    {
                        sb.Append(spacer);
                    }
                }

                return sb.ToString();
            }
        }

        public static List<string> WordNodeListAsStringList(List<WordNode> words)
        {
            List<string> namelist = new List<string>();
            foreach (WordNode word in words)
            {
                namelist.Add(word.ToString());
            }

            return namelist;
        }


    }


}
