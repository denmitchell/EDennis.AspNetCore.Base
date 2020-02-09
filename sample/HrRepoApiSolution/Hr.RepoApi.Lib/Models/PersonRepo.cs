using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using System.Linq;
using System.Threading.Tasks;

namespace Hr.RepoApi.Models {
    public class PersonRepo : Repo<Person, HrContext> {
        public PersonRepo(DbContextProvider<HrContext> provider, 
            IScopeProperties scopeProperties) 
            : base(provider, scopeProperties) {
        }

        public override void Delete(params object[] keyValues) {
            var relatedAddresses = Context.Addresses.Where(e => e.PersonId == (int)keyValues[0]);
            foreach (var address in relatedAddresses)
                Context.Addresses.Remove(address);
            Context.SaveChanges();
            base.Delete(keyValues);
        }

        public override async Task DeleteAsync(params object[] keyValues) {
            var relatedAddresses = Context.Addresses.Where(e => e.PersonId == (int)keyValues[0]);
            foreach (var address in relatedAddresses)
                Context.Addresses.Remove(address);
            await Context.SaveChangesAsync();
            base.Delete(keyValues);
        }
    }
}
