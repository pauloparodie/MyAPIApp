using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyApp.BusinessContract;
using MyApp.DomainModel.Entities;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using System.Web.Http.Filters;



namespace MyAPIApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MyAPIAppController : ApiController
    {
        IBusinessManager _businessManager;

        public MyAPIAppController(IBusinessManager businessManager)
        {
            _businessManager = businessManager;
        }


        [Route("Api/Pessoas")]
        [Route("")]
        public async Task<Object> GetPessoas()
        {
            // awaitTask.Run(() => JsonConvert.SerializeObject(list));         
            /*var list = await _businessManager.GetPessoasAsync();
            list.ForEach(x => x.Animals.ForEach(y => y.Dono = null));
            return list;*/

            var list = (await _businessManager.GetPessoasAsync()).
                        Select(x => new { Cod = x.Cod, Name = x.Name, Pais = x.Pais, Age = x.Age, PhotoName = x.PhotoName, AnimalsCount = x.Animals.Count }).ToList();
            return list;

        }

        [Route("Api/Pessoa/{id:int:min(1)}")]
        //[AuthenticatedFilter]
        public async Task<Object> GetPessoa(int id = 0)
        {
            var pessoaDb = await _businessManager.GetPessoaAsync(id);
            if (pessoaDb == null)
            {
                throw new Exception("Person not found");
            }

            return new
            {
                Cod = pessoaDb.Cod,
                Name = pessoaDb.Name,
                Pais = pessoaDb.Pais,
                Age = pessoaDb.Age,
                PhotoName = pessoaDb.PhotoName,
                AnimalsCount = pessoaDb.Animals.Count
            };
        }

        [Route("Api/Pessoa/Save")]
        [HttpPost]
        [HttpPut]
        //[AuthenticatedFilter]
        public async Task SavePessoa(Pessoa pessoa)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception((ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage) ?? "Invalid request");
            }

            Pessoa pessoaDb = null;
            if (pessoa.Cod > 0)
            {
                pessoaDb = await _businessManager.GetPessoaAsync(pessoa.Cod);
                if (pessoaDb == null)
                {
                    throw new Exception("Person not found");
                }
            }
            else if (pessoa.Cod == 0)
            {
                pessoaDb = new Pessoa();
            }
            else
            {
                throw new Exception("Invalid person cod");
            }

            pessoaDb.Age = pessoa.Age;
            pessoaDb.Name = pessoa.Name;
            pessoaDb.Pais = pessoa.Pais;

            if (pessoaDb.Cod == 0)
            {
                await _businessManager.CreatePessoaAsync(pessoaDb);
            }
            else
            {
                await _businessManager.EditPessoaAsync(pessoaDb);
            }
        }

        [Route("Api/Pessoa/Delete/{id:int:min(1)}")]
        //[AuthenticatedFilter]
        public async Task DeletePessoa(int id)
        {
            try
            {
                await _businessManager.DeletePessoaAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool disposedValue = false; // To detect redundant calls
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _businessManager.Dispose();
                }
                disposedValue = true;
            }
        }
    }
}
