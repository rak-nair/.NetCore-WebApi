using LimespotAssignment.Controllers;
using LimespotAssignment.Data.Domain.Entities;
using LimespotAssignment.Data.Domain.Interfaces;
using LimespotAssignment.Models.Input;
using LimespotAssignment.Services.Interfaces;
using LimespotAssignment.Services.ResponseMessages;
using LimespotAssignment.Tests.Class;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LimespotAssignment.Models;
using LimespotAssignment.Models.Output;
using Xunit;

namespace LimespotAssignment.Tests
{
    public class TransformersControllerShould
    {
        TransformersController _sut;
        Mock<IDatastore> _mockDataStore;
        Mock<IWarResolver> _mockWarResolver;
        Mock<ModelFactory> _mockModelFactory;


        public TransformersControllerShould()
        {
            _mockDataStore = new Mock<IDatastore>();
            _mockWarResolver = new Mock<IWarResolver>();
            _mockModelFactory = new Mock<ModelFactory>();
            _sut = new TransformersController(_mockDataStore.Object, _mockWarResolver.Object, _mockModelFactory.Object);
        }

        [Fact]
        public async void AddOperation_ValidInput_Success()
        {
            _mockDataStore.Setup(x => x.AddTransformerAsync(It.IsAny<ITransformer>()))
                .ReturnsAsync(new Transformer { ID = 1 });

            var result = await _sut.AddAutobot(new TransformerViewModel() { Name = "Grimlock", Rank = 2 });

            var createdAtRouteResult = result as CreatedAtRouteResult;
            Assert.NotNull(createdAtRouteResult);
            Assert.IsAssignableFrom<ITransformer>(createdAtRouteResult.Value);
            Assert.Equal(1, ((ITransformer)createdAtRouteResult.Value).ID);

        }

        [Theory, ClassData(typeof(InvalidTransformerAddData))]
        public async void AddOperation_InvalidInput_BadRequest(string name, int? rank)
        {
            var model = new TransformerViewModel() { Name = name, Rank = rank ?? null };
            SimulateValidation(model);

            var result = await _sut.AddAutobot(model);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(((BadRequestObjectResult)result).Value, Responses.ERROR_INVALID_INPUTS_ADD_TRANSFORMER);
        }

        [Fact]
        public async void GetOperation_ValidID_Success()
        {
            SetupMockTransformerLookup(1, "Grimlock", 7);

            var result = await _sut.GetTransformer(1);

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.IsAssignableFrom<ITransformer>(okObjectResult.Value);
            Assert.Equal(1, ((ITransformer)okObjectResult.Value).ID);
            Assert.Equal("Grimlock", ((ITransformer)okObjectResult.Value).Name);
        }

        [Fact]
        public async void GetOperation_InvalidID_BadRequest()
        {
            _mockDataStore.Setup(x => x.GetTransformerDetailsAsync(It.Is((int y) => y == 0)))
                .ReturnsAsync(() => null);

            var result = await _sut.GetTransformer(0);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(((BadRequestObjectResult)result).Value, Responses.INVALID_TRANSFORMER_ID);
        }

        [Fact]
        public async void UpdateOperation_ValidID_Success()
        {
            SetupMockTransformerLookup(1, "Grimlock", 7);
            var transformerUpdateViewModel = new TransformerUpdateViewModel
            {
                Name = "Grimlock1",
                Rank = 8
            };
            _mockDataStore.Setup(x => x.UpdateTransformerAsync(1, It.IsAny<ITransformer>()))
                .ReturnsAsync(new Transformer { Name = "Grimlock1", Rank = 8, ID = 1 });

            var result = await _sut.UpdateTransformer(1, transformerUpdateViewModel);

            Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<ITransformer>(((OkObjectResult)result).Value);
            Assert.Equal(1, ((ITransformer)((OkObjectResult)result).Value).ID);
            Assert.Equal("Grimlock1", ((ITransformer)((OkObjectResult)result).Value).Name);
            Assert.Equal(8, ((ITransformer)((OkObjectResult)result).Value).Rank);
        }

        [Fact]
        public void UpdateOperation_InvalidID_BadRequest()
        {
            GetOperation_InvalidID_BadRequest();
        }

        [Fact]
        public async void RemoveOperation_ValidID_Success()
        {
            SetupMockTransformerLookup(1, "Grimlock", 7);
            _mockDataStore.Setup(x => x.RemoveTransformerAsync(1))
                .ReturnsAsync(true);

            var result = await _sut.RemoveTransformer(1);

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent,((StatusCodeResult)result).StatusCode);
        }

        [Fact]
        public void RemoveOperation_InvalidID_BadRequest()
        {
            GetOperation_InvalidID_BadRequest();
        }

        [Fact]
        public async void ListOperation_Autobots_Success()
        {
            _mockDataStore.Setup(x => x.GetAllAutobotsAsync())
                .ReturnsAsync(new List<ITransformer>
                {
                    new Transformer{ID = 1, Name = "Grimlock",Rank = 7},
                    new Transformer{ID = 2, Name = "Hound",Rank = 5}
                });

            var result = await _sut.ListAutobots();

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<ITransformer>>(((OkObjectResult)result).Value);
            var collection = ((OkObjectResult) result).Value as List<ITransformer>;
            Assert.Equal(2,collection.Count);
            Assert.Equal("Grimlock",collection[0].Name);
            Assert.Equal("Hound", collection[1].Name);
        }

        [Fact]
        public async void ListOperation_Decepticons_Success()
        {
            _mockDataStore.Setup(x => x.GetAllDecepticonsAsync())
                .ReturnsAsync(new List<ITransformer>
                {
                    new Transformer{ID = 1, Name = "Fallen",Rank = 7},
                    new Transformer{ID = 2, Name = "Frenzy",Rank = 5}
                });

            var result = await _sut.ListDecepticons();

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<ITransformer>>(((OkObjectResult)result).Value);
            var collection = ((OkObjectResult)result).Value as List<ITransformer>;
            Assert.Equal(2, collection.Count);
            Assert.Equal("Fallen", collection[0].Name);
            Assert.Equal("Frenzy", collection[1].Name);
        }

        [Fact]
        public async void OverallScoreOperation_ValidID_Success()
        {
            SetupMockTransformerLookup(1, "Grimlock", 7);
            _mockDataStore.Setup(x => x.GetTransformerScoreAsync(1))
                .ReturnsAsync(15);

            var result = await _sut.GetOverallScore(1);

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<int>(((OkObjectResult)result).Value);
            Assert.Equal(15, ((OkObjectResult)result).Value);
        }

        [Fact]
        public void OverallScoreOperation_InvalidID_BadRequest()
        {
            GetOperation_InvalidID_BadRequest();
        }

        [Fact]
        public async void WageWar_Success()
        {
            _mockDataStore.Setup(x => x.GetAllTransformersAsync())
                .ReturnsAsync(new List<ITransformer>());

            _mockWarResolver.Setup(x => x.WageWarAsync(It.IsAny<List<ITransformer>>()))
                .ReturnsAsync(new WarResult{Message = Responses.NO_COMBATANTS});

            var result = await _sut.WageWar();

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<WarResult>(((OkObjectResult)result).Value);
            Assert.Equal(Responses.NO_COMBATANTS, ((WarResult)((OkObjectResult)result).Value).Message);
        }

        //Set up a mock transformer driven by the input parameters.
        void SetupMockTransformerLookup(int transformerID, string transformerName, int transformerRank)
        {
            _mockDataStore.Setup(x => x.GetTransformerDetailsAsync(It.Is((int y) => y == 1)))
                .Returns(Task.FromResult<ITransformer>(new Transformer()
                {
                    ID = transformerID,
                    Name = transformerName,
                    Rank = transformerRank
                }));
        }

        // Mimic the behaviour of the model binder which is responsible for Validating the Model
        void SimulateValidation(object model)
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                _sut.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }
    }
}
