using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class CategoryCascadeDeleteStrategy : CascadeDeleteStrategy<Category>
    {
        public CategoryCascadeDeleteStrategy(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> Delete(Category record)
        {
            Database.Categories.Delete(record.Id);

            var roots = Database.Roots.GetAll().Where(x => x.CategoryIds.Contains(record.Id));

            foreach (var root in roots)
            {
                root.CategoryIds.Remove(record.Id);
                Database.Roots.Update(root);
            }

            return Database.Categories.GetAll()
                .Where(x => x.ParentId == record.Id)
                .ToList();
        }
    }
}
