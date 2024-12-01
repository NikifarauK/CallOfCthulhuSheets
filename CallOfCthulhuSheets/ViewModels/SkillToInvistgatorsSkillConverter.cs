using CallOfCthulhuSheets.Models;

namespace CallOfCthulhuSheets.ViewModels
{
    public class SkillToInvistgatorsSkillConverter : BaseViewModel
    {
        public SkillToInvistgatorsSkillConverter(Skill skill)
        {
            try
            {
                FromSkill = skill;
                ToInvSkill = new InvestigatorsSkills() { Skill = skill };
                ToInvSkill.CurrentSkillValue = skill.BasePoints;
                MinSkillPointValue = skill.BasePoints;
                SkillPoint = skill.BasePoints;
                PreviusSkillPoint = skill.BasePoints;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private Skill fromSkill;
        public Skill FromSkill
        {
            get => fromSkill;
            set => SetProperty(ref fromSkill, value);
        }

        private InvestigatorsSkills toInvSkill;
        public InvestigatorsSkills ToInvSkill
        {
            get => toInvSkill;
            set => toInvSkill = value;
        }

        private int minSkillPointValue;
        public int MinSkillPointValue { get => minSkillPointValue; set => SetProperty(ref minSkillPointValue, value); }

        public double PreviusSkillPoint { get; set; }
        private double skillPoint;

        public double SkillPoint
        {
            get => skillPoint;
            set
            {
                if (value < MinSkillPointValue) return;
                if (SetProperty(ref skillPoint, (int)value))
                {
                    ToInvSkill.CurrentSkillValue = (int)value;
                }
            }
        }

    }
}
