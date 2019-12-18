using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PyramidString.Models;

namespace PyramidString.Pages
{
    public class PageController : Controller
    {
        private readonly ILogger<Index> _logger;

        public PageController(ILogger<Index> logger)
        {
            _logger = logger;
        }

        public void onGet()
        {

        }

        [HttpPost]
        public ActionResult PyramidWord(PyramidStringModel u)
        {
            u.OutputString = IsPyramidWord(u.InputString);
            return RedirectToAction("View");
        }

        bool IsPyramidWord(string str)
        {
            Dictionary<int, List<char>> mpFreqToCharSet = new Dictionary<int, List<char>>();
            Dictionary<char, int> mpCharFreq = new Dictionary<char, int>();
            int maxFreq = 0;
            foreach (var c in str)
            {
                int newFreq = mpCharFreq[c] + 1;
                //remove character from old frequency (Avg. case O(1))
                if (mpFreqToCharSet[newFreq - 1].Count(x=>x == c) > 0)
                {
                    mpFreqToCharSet[newFreq - 1].Remove(c);
                }
                //insert character to new frequency (Avg. case O(1))
                mpFreqToCharSet[newFreq].Add(c);
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
