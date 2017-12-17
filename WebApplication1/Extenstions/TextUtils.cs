using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


public static class Text
{
    public static HtmlString ToHtml(this string htmlValue)
    {
        return new HtmlString(htmlValue ?? "");
    }

    public static string TruncateText(this string fullText, int numberOfCharacters)
    {
        string str = "";

        if (fullText == null)
            return str;

        if (fullText.Length > numberOfCharacters && numberOfCharacters > 0)
        {
            int index = fullText.IndexOf(" ", numberOfCharacters);
            if (index > -1)
                str = String.Format("{0}", fullText.Substring(0, index));
            else
                str = fullText;
        }
        else
            str = fullText;

        Regex regex = new Regex("<.*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        return String.Format("{0}{1}", regex.Replace(str, " ").Trim(), "");//, fullText.Length > numberOfCharacters ? "&#8230;" : "" );
    }

    public static string StripDiacritics(this string accented)
    {
        if (String.IsNullOrEmpty(accented))
            return String.Empty;

        var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
        string strFormD = accented.Normalize(NormalizationForm.FormD);
        return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
    }

    public static string ToSingleSpace(this string s)
    {
        string result;
        if (String.IsNullOrEmpty(s))
            result = String.Empty;
        else
            result = Regex.Replace(s, "\\s+", " ");

        return result.Trim();
    }
    public static string ToSingleBreakline(this string s)
    {
        string result;
        if (String.IsNullOrEmpty(s))
            result = String.Empty;
        else
            s = (result = Regex.Replace(s, "(?:\\r\\n|[\\r\\n])", "\n"));

        return result;
    }

    public static string ToStripString(this string word)
    {
        string result = word.ToSingleSpace();

        word = (result = word.Replace(",", ", "));
        word = (result = word.Replace(".", ". "));
        word = (result = word.Replace(" ,", ","));
        word = (result = word.Replace(" .", "."));

        var arr = word.Split(new char[] { '.' }, StringSplitOptions.None);

        word = result = String.Join(". ", arr.Select(str => str.MakeInitialCaps()).ToArray());
        word = (result = word.Replace(" .", "."));

        return result.Trim();
    }

    public static string ToFriendlyUrl(this string word)
    {
        if (!string.IsNullOrEmpty(word)&& word.Length>150)
        {
            word = word.Substring(0, 150);
        }
        var regEx = word.StripDiacritics().ToLower().Replace(" ", "-");
        regEx = Regex.Replace(regEx, @"\-+", "-");
        regEx = Regex.Replace(regEx, @"[^\w+\-]", "");
        return regEx;
    }


    public static string MakeInitialCaps(this string word)
    {
        string result = word.Trim();

        if (String.IsNullOrEmpty(result))
            return String.Empty;

        return String.Format("{0}{1}", result.Substring(0, 1).ToUpper(), result.Substring(1)).Trim();
    }

    public static string SubStr(this string s, int length)
    {
        if (String.IsNullOrEmpty(s))
            return s;
        var r = s;
        if ((length > 0) && (s.Length > length))
        {
            if (s[length - 1] != ' ')
            {
                r = r.Substring(0, length);
                r = r.Substring(0, r.LastIndexOf(' ')).Trim() + "...";
            }
            else
                r = r.Substring(0, length).Trim() + "...";
        }
        return r;
    }

    public static string Base64Encode(this string plainText)
    {
        var _byte = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(_byte);
    }

    public static string Base64Decode(string base64EncodedData)
    {
        var _byte = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(_byte);
    }
}