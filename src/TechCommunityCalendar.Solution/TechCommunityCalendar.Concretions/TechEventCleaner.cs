namespace TechCommunityCalendar.Concretions
{
    public class TechEventCleaner
    {
        public static string MakeFriendlyBranchName(string eventName)
        {
            // Some branch name rules from https://wincent.com/wiki/Legal_Git_branch_names
            // Others are my choice

            var branchName = eventName.ToLower()
                .Trim()
                .Replace("-", " ") // Remove any existing dashes
                .Replace(".", " ")
                .Replace("~", " ")
                .Replace("^", " ")
                .Replace(":", " ")
                .Replace("\\", " ")
                .Replace("/", " ")
                .Replace(";", " ")
                .Replace(",", " ")
                .Replace(".lock", " ")
                .Replace(" ", "-")
                .Replace("--", "-")
                .Replace("--", "-");

            if (branchName.EndsWith("-"))
            {
                branchName = branchName.Substring(0, branchName.Length - 1);
            }

            return branchName;
        }
    }
}
