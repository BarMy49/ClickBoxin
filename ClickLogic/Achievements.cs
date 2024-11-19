using Microsoft.Toolkit.Uwp.Notifications;
namespace ClickBoxin
{
    public class Achievements
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; set; }

        public Achievements(string name, string description)
        {
            Name = name;
            Description = description;
            IsUnlocked = false;
        }

        public void Unlock()
        {
            IsUnlocked = true;
#if WINDOWS_UWP
            new ToastContentBuilder()
                .AddText("Achievement Unlocked!")
                .AddText($"{Name}: {Description}")
                .Show();
#else
            Console.WriteLine($"Achievement Unlocked: {Name} \n{Description}");
#endif
        }
        public void Lock()
        {
            IsUnlocked = false;
        }
    }
}