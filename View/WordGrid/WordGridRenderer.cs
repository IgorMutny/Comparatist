using Comparatist.Application.Cache;
using Comparatist.View.Common;
using Comparatist.View.Fonts;
using Comparatist.View.WordGrid.BinderRenderers;
using Comparatist.View.WordGrid.Binders;

namespace Comparatist.View.WordGrid
{
    internal class WordGridRenderer : Renderer<DataGridView>
    {
        private Dictionary<CategoryBinder, DataGridViewRow> _categories = new();
        private Dictionary<RootBinder, DataGridViewRow> _roots = new();
        private Dictionary<StemBinder, DataGridViewRow> _stems = new();
        private Dictionary<WordBinder, DataGridViewCell> _words = new();
        private Dictionary<Guid, DataGridViewColumn> _columns = new();

        private ColumnRenderer _columnRenderer;
        private CategoryBinderRenderer _categoryRenderer;
        private RootBinderRenderer _rootRenderer;
        private StemBinderRenderer _stemRenderer;
        private WordBinderRenderer _wordRenderer;
        private RichTextCellPainter _richTextCellPainter;

        public WordGridRenderer(DataGridView control) : base(control)
        {
            _columnRenderer = new ColumnRenderer(control, _columns);
            _categoryRenderer = new CategoryBinderRenderer(control, _categories);
            _rootRenderer = new RootBinderRenderer(control, _categories, _roots);
            _stemRenderer = new StemBinderRenderer(control, _roots, _stems);
            _wordRenderer = new WordBinderRenderer(control, _columns, _stems, _words);
            _richTextCellPainter = new(Control);
        }

        public override void Dispose()
        {
            _richTextCellPainter.Dispose();
        }

        public void Reset()
        {
            Control.Columns.Clear();
            Control.Rows.Clear();
            _categories.Clear();
            _roots.Clear();
            _stems.Clear();
            _words.Clear();
            _columns.Clear();

			Control.Font = FontManager.Instance.Font;
		}

        public void CreateColumns(List<CachedLanguage> languages)
        {
            _columnRenderer.CreateColumns(languages);
        }

        public void Add<T>(T binder)
            where T : class, IBinder
        {
            switch (binder)
            {
                case (CategoryBinder categoryBinder):
                    _categoryRenderer.Add(categoryBinder);
                    break;
                default: throw new NotSupportedException();
            }
        }

        public void Add<T1, T2>(T1 binder, T2 parent)
            where T1 : class, IBinder where T2 : class, IBinder
        {
            switch (binder, parent)
            {
                case (RootBinder rootBinder, CategoryBinder categoryBinder):
                    _rootRenderer.Add(rootBinder, categoryBinder);
                    break;
                case (StemBinder stemBinder, RootBinder rootBinder):
                    _stemRenderer.Add(stemBinder, rootBinder);
                    break;
                case (WordBinder wordBinder, StemBinder stemBinder):
                    _wordRenderer.Add(wordBinder, stemBinder);
                    break;
                default: throw new NotSupportedException();
            }
        }

        public void Update<T>(T binder)
            where T : class, IBinder
        {
            switch (binder)
            {
                case (CategoryBinder categoryBinder):
                    break;
                case (RootBinder rootBinder):
                    _rootRenderer.Update(rootBinder);
                    break;
                case (StemBinder stemBinder):
                    _stemRenderer.Update(stemBinder);
                    break;
                case (WordBinder wordBinder):
                    _wordRenderer.Update(wordBinder);
                    break;
                default: throw new NotSupportedException();
            }
        }

        public void Move<T>(T binder, T? previousBinder) where T : class, IBinder
        {
            switch (binder, previousBinder)
            {
                case (RootBinder rootBinder, RootBinder previousRootBinder):
                    _rootRenderer.Move(rootBinder, previousRootBinder);
                    break;
                case (RootBinder rootBinder, null):
                    _rootRenderer.Move(rootBinder, null);
                    break;
                case (StemBinder stemBinder, StemBinder previousStemBinder):
                    _stemRenderer.Move(stemBinder, previousStemBinder);
                    break;
                case (StemBinder stemBinder, null):
                    _stemRenderer.Move(stemBinder, null);
                    break;
                default: throw new NotSupportedException();
            }
        }

        public void Remove<T>(T binder) where T : IBinder
        {
            switch (binder)
            {
                case RootBinder rootBinder: _rootRenderer.Remove(rootBinder); break;
                case StemBinder stemBinder: _stemRenderer.Remove(stemBinder); break;
                case WordBinder wordBinder: _wordRenderer.Remove(wordBinder); break;
                default: throw new NotSupportedException();
            }
        }
    }
}

