using System;
using System.Collections.Generic;
using  System.Text;

namespace WordLineage
{
    public static class WL_Utility
    {
        public static string FormatStringListForDisplay(List<string> list,
            bool listIsNumbered = false, string spacer = " ", string prepend = "", string append = "")
        {
            if (list.Count <= 0)
            {   // return empty string if nothing in list
                return "";
            }
            else
            {
                StringBuilder sb = new("");

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
            List<string> namelist = new();
            foreach (WordNode word in words)
            {
                namelist.Add(word.ToString());
            }

            return namelist;
        }

        public static WordFamily GenerateDogNameList()
        {
            WordFamily testFamily = new("Names I Call My Dog");

            // create wordnodes
            // tier 0 (roots)
            WordNode puppy = new("puppy"); WordNode stinky = new("stinky baby");
            WordNode doggo = new("doggo"); WordNode grumpy = new("grumpy");
            // tier 1
            WordNode buppy = new("buppy"); WordNode puppo = new("puppo");
            WordNode stonky = new("stonky"); WordNode babbi = new("babbi");
            WordNode grumby = new("grumby");
            // tier 2
            WordNode buppo = new("buppo"); WordNode bubbo = new("bubbo");
            WordNode stimky = new("stimky"); WordNode grumbis = new("gumbis");

            // Add Connections
            // tier 0 -> 1
            puppy.AddConnections(buppy, puppo);
            doggo.AddConnections(buppy, puppo);
            stinky.AddConnections(stonky, babbi);
            grumpy.AddConnections(grumby);
            // tier 1 -> 2
            buppy.AddConnections(buppo, bubbo);
            puppo.AddConnections(buppo, bubbo);
            stonky.AddConnection(stimky);
            babbi.AddConnections(stimky, grumbis);
            grumby.AddConnection(grumbis);

            // Add Words to Family (random order)
            testFamily.AddWords(grumbis, buppo, stimky, stinky, grumpy, puppy, grumby,
                puppo, babbi, bubbo, doggo, buppy, stonky);

            return testFamily;
        }

    }


}
