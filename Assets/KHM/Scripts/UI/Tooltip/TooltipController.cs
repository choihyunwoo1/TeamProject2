using System.Collections.Generic;
using UnityEngine;

namespace hm
{
    public class TooltipController : MonoBehaviour
    {
        [SerializeField] private BuffTooltipUI buffTooltip;
        [SerializeField] private ItemTooltipUI itemTooltip;
        [SerializeField] private SkillTooltipUI skillTooltip;

        private Dictionary<TooltipType, TooltipUIBase> tooltipMap;

        private void Awake()
        {
            tooltipMap = new Dictionary<TooltipType, TooltipUIBase>
        {
            { TooltipType.Buff, buffTooltip },
            { TooltipType.Item, itemTooltip },
            { TooltipType.Skill, skillTooltip }
        };
        }

        public void Show(ITooltipData data)
        {
            HideAll();
            if (tooltipMap.TryGetValue(data.Type, out var tooltip))
            {
                tooltip.Show(data);
            }
        }

        public void HideAll()
        {
            foreach (var tooltip in tooltipMap.Values)
            {
                tooltip.Hide();
            }
        }
    }
}