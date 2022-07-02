using System.Windows;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string xmlContent = Properties.Resources.syntax;
            HighlighterManager.Instance.LoadSyntaxXML(xmlContent);
            box.CurrentHighlighter = HighlighterManager.Instance.Highlighters["MySyntax"];
        }
    }
}
