using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PokemonTypeCalculator
{
    public enum PokeType
    {
        None,
        Normal,
        Fire,
        Water,
        Electric,
        Grass,
        Ice,
        Fighting,
        Poison,
        Ground,
        Flying,
        Psychic,
        Bug,
        Rock,
        Ghost,
        Dragon,
        Dark,
        Steel,
        Fairy
    }

    public class TypeItem
    {
        public double Effectiveness { get; set; }
        public double Resistance { get; set; }
        public string Name { get; set; }
        public string BGColor { get; set; }
        public string ResistColor { get; set; }
        public string TypeColor { get; set; }
    }

    public class TypeSelectItem
    {
        public string Name { get; set; }
        public string TypeColor { get; set; }
        public PokeType type { get; set; }
        public override string ToString() { return Name; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PokeType> moveTypes;
        List<PokeType> pokeTypes;
        Dictionary<PokeType, Dictionary<PokeType, double>> weaknesses;
        Dictionary<PokeType, string> typeColors;

        public MainWindow()
        {
            InitializeComponent();

            var bc = new BrushConverter();
            typeColors = new Dictionary<PokeType, string> {
                {PokeType.None, "gray" },
                {PokeType.Normal, "#FFAAAA99" },
                {PokeType.Fire, "#FFFF4422" },
                {PokeType.Water, "#FF3399FF" },
                {PokeType.Electric, "#FFffcc33" },
                {PokeType.Grass, "#FF77cc55" },
                {PokeType.Ice, "#FF66ccff" },
                {PokeType.Fighting, "#FFbb5544" },
                {PokeType.Poison, "#FFaa5599" },
                {PokeType.Ground, "#FFddbb55" },
                {PokeType.Flying, "#FF8899ff" },
                {PokeType.Psychic, "#FFff5599" },
                {PokeType.Bug, "#FFaabb22" },
                {PokeType.Rock, "#FFbbaa66" },
                {PokeType.Ghost, "#FF6666bb" },
                {PokeType.Dragon, "#FF7766ee" },
                {PokeType.Dark, "#FF775544" },
                {PokeType.Steel, "#FFaaaabb" },
                {PokeType.Fairy, "#FFee99ee" },
            };

            moveTypes = new List<PokeType>{ PokeType.None, PokeType.None, PokeType.None, PokeType.None };
            pokeTypes = new List<PokeType> { PokeType.None, PokeType.None };

            foreach (PokeType type in Enum.GetValues(typeof(PokeType)))
            {
                var itm = new TypeSelectItem { Name = type.ToString(), TypeColor = typeColors[type], type = type };
                PokemonType1.Items.Add(itm);
                PokemonType2.Items.Add(itm);
                MoveType1.Items.Add(itm);
                MoveType2.Items.Add(itm);
                MoveType3.Items.Add(itm);
                MoveType4.Items.Add(itm);
            }

            weaknesses = new Dictionary<PokeType, Dictionary<PokeType, double>>();
            weaknesses[PokeType.None] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 0 },
                {PokeType.Fire, 0 },
                {PokeType.Water, 0 },
                {PokeType.Electric, 0 },
                {PokeType.Grass, 0 },
                {PokeType.Ice, 0 },
                {PokeType.Fighting, 0 },
                {PokeType.Poison, 0 },
                {PokeType.Ground, 0 },
                {PokeType.Flying, 0 },
                {PokeType.Psychic, 0 },
                {PokeType.Bug, 0 },
                {PokeType.Rock, 0 },
                {PokeType.Ghost, 0 },
                {PokeType.Dragon, 0 },
                {PokeType.Dark, 0 },
                {PokeType.Steel, 0 },
                {PokeType.Fairy, 0 },
            };

            weaknesses[PokeType.Normal] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, 1 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 1 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, .5 },
                {PokeType.Ghost, 0 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, .5 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Fire] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, .5 },
                {PokeType.Water, .5 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 2 },
                {PokeType.Ice, 2 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 2 },
                {PokeType.Rock, .5 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, .5 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, 2 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Water] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, 2 },
                {PokeType.Water, .5 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, .5 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, 2 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, 2 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, .5 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, 1 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Electric] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, 1 },
                {PokeType.Water, 2 },
                {PokeType.Electric, .5 },
                {PokeType.Grass, .5 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, 0 },
                {PokeType.Flying, 2 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, 1 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, .5 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, 1 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Grass] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, .5 },
                {PokeType.Water, 2 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, .5 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, .5 },
                {PokeType.Ground, 2 },
                {PokeType.Flying, .5 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, .5 },
                {PokeType.Rock, 2 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, .5 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, .5 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Ice] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, .5 },
                {PokeType.Water, .5 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 2 },
                {PokeType.Ice, .5 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, 2 },
                {PokeType.Flying, 2 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, 1 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, 2 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, .5 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Fighting] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 2 },
                {PokeType.Fire, 1 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 1 },
                {PokeType.Ice, 2 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, .5 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, .5 },
                {PokeType.Psychic, .5 },
                {PokeType.Bug, .5 },
                {PokeType.Rock, 2 },
                {PokeType.Ghost, 0 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, 2 },
                {PokeType.Steel, 2 },
                {PokeType.Fairy, .5 },
            };

            weaknesses[PokeType.Poison] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, 1 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 2 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, .5 },
                {PokeType.Ground, .5 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, .5 },
                {PokeType.Ghost, .5 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, 0 },
                {PokeType.Fairy, 2 },
            };

            weaknesses[PokeType.Ground] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, 2 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 2 },
                {PokeType.Grass, .5 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, 2 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, 0 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, .5 },
                {PokeType.Rock, 2 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, 2 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Flying] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, 1 },
                {PokeType.Water, 1 },
                {PokeType.Electric, .5 },
                {PokeType.Grass, 2 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 2 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 2 },
                {PokeType.Rock, .5 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, .5 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Psychic] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, 1 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 1 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 2 },
                {PokeType.Poison, 2 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, .5 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, 1 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, 0 },
                {PokeType.Steel, .5 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Bug] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, .5 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 2 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, .5 },
                {PokeType.Poison, .5 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, .5 },
                {PokeType.Psychic, 2 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, 1 },
                {PokeType.Ghost, .5 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, 2 },
                {PokeType.Steel, .5 },
                {PokeType.Fairy, .5 },
            };

            weaknesses[PokeType.Rock] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, 2 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 1 },
                {PokeType.Ice, 2 },
                {PokeType.Fighting, .5 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, .5 },
                {PokeType.Flying, 2 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 2 },
                {PokeType.Rock, 1 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, .5 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Ghost] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 0 },
                {PokeType.Fire, 1 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 1 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, 2 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, 1 },
                {PokeType.Ghost, 2 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, .5 },
                {PokeType.Steel, 1 },
                {PokeType.Fairy, 1 },
            };

            weaknesses[PokeType.Dragon] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, 1 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 1 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, 1 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, 2 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, .5 },
                {PokeType.Fairy, 0 },
            };

            weaknesses[PokeType.Dark] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, 1 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 1 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, .5 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, 2 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, 1 },
                {PokeType.Ghost, 2 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, .5 },
                {PokeType.Steel, 1 },
                {PokeType.Fairy, .5 },
            };

            weaknesses[PokeType.Steel] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, .5 },
                {PokeType.Water, .5 },
                {PokeType.Electric, .5 },
                {PokeType.Grass, 1 },
                {PokeType.Ice, 2 },
                {PokeType.Fighting, 1 },
                {PokeType.Poison, 1 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, 2 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, 1 },
                {PokeType.Dark, 1 },
                {PokeType.Steel, .5 },
                {PokeType.Fairy, 2 },
            };

            weaknesses[PokeType.Fairy] = new Dictionary<PokeType, double> {
                {PokeType.None, 0 },
                {PokeType.Normal, 1 },
                {PokeType.Fire, .5 },
                {PokeType.Water, 1 },
                {PokeType.Electric, 1 },
                {PokeType.Grass, 1 },
                {PokeType.Ice, 1 },
                {PokeType.Fighting, 2 },
                {PokeType.Poison, .5 },
                {PokeType.Ground, 1 },
                {PokeType.Flying, 1 },
                {PokeType.Psychic, 1 },
                {PokeType.Bug, 1 },
                {PokeType.Rock, 1 },
                {PokeType.Ghost, 1 },
                {PokeType.Dragon, 2 },
                {PokeType.Dark, 2 },
                {PokeType.Steel, .5 },
                {PokeType.Fairy, 1 },
            };

            RefreshList();
        }

        string getColor(double effectiveness)
        {
            if (effectiveness == 0) return "gray";
            if (effectiveness <= .25) return "red";
            if (effectiveness <= .5) return "orange";
            if (effectiveness >= 400) return "LightBlue";
            if (effectiveness >= 4) return "green";
            if (effectiveness >= 2) return "LightGreen";
            return "White";
        }

        private void RefreshList()
        {
            ResultView.Items.Clear();
            foreach (PokeType restype in Enum.GetValues(typeof(PokeType)))
            {
                if (restype == PokeType.None)
                {
                    continue;
                }
                double effect = 0;
                foreach (var atktype in moveTypes)
                {
                    effect = Math.Max(effect, weaknesses[atktype][restype]);
                }

                double resist = 1.0;
                foreach (var deftype in pokeTypes)
                {
                    if (deftype == PokeType.None) continue;
                    resist *= weaknesses[restype][deftype];
                }

                ResultView.Items.Add(new TypeItem { 
                    Effectiveness = effect, 
                    Name = restype.ToString(), 
                    BGColor = getColor(effect), 
                    TypeColor = typeColors[restype],
                    Resistance = resist,
                    ResistColor = getColor(resist)});
            }
        }

        public static Color GetDarker(Color color, double coef)
        {
            return Color.FromArgb((byte)(color.A), (byte)(color.R * coef), (byte)(color.G * coef),
                (byte)(color.B * coef));
        }

        private void updateButton(ComboBox box)
        {
            var typ = ((TypeSelectItem)box.SelectedItem).type;
            var convert = new BrushConverter();
            var col = convert.ConvertFromString(typeColors[typ]) as SolidColorBrush;
            var darker = new SolidColorBrush(GetDarker(col.Color, .7));
            box.Background = col;
            box.BorderBrush = darker;
            ResultView.Focus();

            ToggleButton toggleButton = FindVisualChild<ToggleButton>(box);
            if (toggleButton != null)
            {
                toggleButton.Background = col;
                toggleButton.BorderBrush = darker;
                Path path = FindVisualChild<Path>(toggleButton);
                if (path != null)
                {
                    path.Fill = darker;
                }
            }
        }

        private void OnCloseDropdown(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;
            var indexchar = box.Name.Last();
            int index = indexchar - '1';
            if (box.SelectedItem != null && index >= 0 && index < moveTypes.Count)
            {
                var typ = ((TypeSelectItem)box.SelectedItem).type;
                moveTypes[index] = typ;
                updateButton(box);
                RefreshList();
            }
        }

        private void OnClosePokeDropdown(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;
            var indexchar = box.Name.Last();
            int index = indexchar - '1';
            if (box.SelectedItem != null && index >= 0 && index < moveTypes.Count)
            {
                var typ = ((TypeSelectItem)box.SelectedItem).type;
                pokeTypes[index] = typ;
                updateButton(box);
                RefreshList();
            }
        }

        private static T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (child != null)
                {
                    T correctlyTyped = child as T;
                    if (correctlyTyped != null)
                        return correctlyTyped;

                    T descendent = FindVisualChild<T>(child);
                    if (descendent != null)
                        return descendent;
                }
            }
            return null;
        }
    }
}
