using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class CategoryCascadeDeleteStrategy : CascadeDeleteStrategy<Category>
    {
        public CategoryCascadeDeleteStrategy(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> Delete(Category record)
        {
            var categoryRepo = Database.GetRepository<Category>();
            var rootRepo = Database.GetRepository<Root>();

            categoryRepo.Delete(record.Id);

            var updatedRoots = rootRepo.GetAll().Where(x => x.CategoryIds.Contains(record.Id));

            foreach (var root in updatedRoots)
            {
                root.CategoryIds.Remove(record.Id);
                rootRepo.Update(root);
            }

            return categoryRepo.GetAll().Where(x => x.ParentId == record.Id).ToList();
        }
    }
}
