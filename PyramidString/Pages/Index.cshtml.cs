using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PyramidString.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<Index> _logger;
        [BindProperty(SupportsGet = true)]
        public string InputString { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool OutputString { get; set; }
        public IndexModel(ILogger<Index> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (!String.IsNullOrWhiteSpace(InputString))
            {
                OutputString = isPyramidWord(InputString);
                ViewData["OutputString"] = OutputString;
            }
        }


        bool isPyramidWord(string str)
        {
            Dictionary<int, List<char>> mpFreqToCharSet = new Dictionary<int, List<char>>();
            Dictionary<char, int> mpCharFreq = new Dictionary<char, int>();
            int maxFreq = 0;
            foreach (var c in str)
            {
                int newFreq = 1;
                if (mpCharFreq.ContainsKey(c))
                {
                    newFreq = mpCharFreq[c] + 1;
                }
                else
                {
                    mpCharFreq.Add(c, 1);
                }
                //remove character from old frequency (Avg. case O(1))
                if (mpFreqToCharSet.ContainsKey(newFreq - 1) && mpFreqToCharSet[newFreq - 1].Count(x=>x == c) > 0)
                {
                    mpFreqToCharSet[newFreq - 1].Remove(c);
                }
                //insert character to new frequency (Avg. case O(1))
                if (!mpFreqToCharSet.ContainsKey(newFreq))
                {
                    mpFreqToCharSet.Add(newFreq, new List<char> { c });
                }
                else {
                    mpFreqToCharSet[newFreq].Add(c);
                }
                //update character frequency map
                mpCharFreq[c] = newFreq;
                maxFreq = Math.Max(maxFreq, newFreq);
            }

            for (int i = 1; i <= maxFreq; i++)
            {
                //if set size is 0, that indicates a gap
                //if set size is more than 1, that indicates a duplicate
                if (mpFreqToCharSet[i].Count() == 0 || mpFreqToCharSet[i].Count() > 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
