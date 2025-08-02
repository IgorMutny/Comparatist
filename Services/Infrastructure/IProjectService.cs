using Comparatist.Core.Records;
using Comparatist.Services.CategoryTree;
using Comparatist.Services.TableCache;

namespace Comparatist.Services.Infrastructure
{
    public interface IProjectService
    {
        Result AddCategory(Category category);
        Result AddLanguage(Language language);
        Result AddRoot(Root root);
        Result AddStem(Stem stem);
        Result AddWord(Word word);
        Result DeleteCategory(Category category);
        Result DeleteLanguage(Language language);
        Result DeleteRoot(Root root);
        Result DeleteStem(Stem stem);
        Result DeleteWord(Word word);
        Result Reorder<T>(IEnumerable<T> records) where T : IOrderableRecord;
        Result<IEnumerable<CachedBlock>> GetAllBlocksByAlphabet();
        Result<CachedBlock> GetBlock(Guid rootId);
        Result<CachedRow> GetRow(Guid stemId);
        Result<IEnumerable<CachedCategoryNode>> GetTree();
        Result<IEnumerable<Language>> GetAllLanguages();
        Result<IEnumerable<Category>> GetAllCategories();
        Result LoadDatabase(string path);
        Result SaveDatabase(string path);
        Result UpdateCategory(Category category);
        Result UpdateLanguage(Language language);
        Result UpdateRoot(Root root);
        Result UpdateStem(Stem stem);
        Result UpdateWord(Word word);
    }
}