using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class CategoryCascadeDeleteStrategy : CascadeDeleteStrategy<Category>
    {
        protected override IEnumerable<IRecord> Delete(Category record)
        {
            record.IsDeleted = true;
            Database.Categories.Update(record);

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
