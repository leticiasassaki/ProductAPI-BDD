using FluentAssertions;
using ProductAPI.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace ProductAPI.Tests.Steps
{
    [Binding]
    public class ProductSteps : BaseTest
    {
        private string ProductEndPoint { get; set; }
        private readonly Product product;
        public ProductSteps()
        {
            ProductEndPoint = $"{ApiUri}api/product";
            product = new Product();
        }

        [Given(@"I input id (.*)")]
        public void GivenIInputId(long id)
        {
            product.Id = id;
        }

        [Given(@"I input name ""(.*)""")]
        public void GivenIInputName(string name)
        {
            product.Name = name;
        }

        [Given(@"The product ""(.*)"" with id (.*) already exists")]
        public void GivenTheProductAlreadyExists(string name, long id)
        {
            product.Id = id;
            product.Name = name;

            var result = Task.Run(async () => await Client.GetAsync($"{ProductEndPoint}/{product.Id}")).Result;
            
            if (result.IsSuccessStatusCode is false)
            { 
                var data = JsonData(product);
                result = Task.Run(async () => await Client.PostAsync(ProductEndPoint, data)).Result;
            }
        }

        [When(@"I request to create product")]
        public void WhenIRequestToCreateProduct()
        {
            try
            {
                var data = JsonData(product);
                var result = Task.Run(async () => await Client.PostAsync(ProductEndPoint, data)).Result;

                result.Should().NotBeNull();
                result.StatusCode.Should().Be(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [When(@"I request to update product name to ""(.*)""")]
        public void WhenIRequestToUpdateProductName(string name)
        {
            try
            {
                product.Name = name;
                var data = JsonData(product);
                var result = Task.Run(async () => await Client.PutAsync($"{ProductEndPoint}/{product.Id}", data)).Result;

                result.Should().NotBeNull();
                result.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Then(@"Validate product ""(.*)"" exists")]
        public void ThenValidateProduct(string name)
        {
            var result = Task.Run(async () => await Client.GetAsync($"{ProductEndPoint}/{product.Id}")).Result;
            var productResult = ObjectData<Product>(result.Content.ReadAsStringAsync().Result);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            productResult.Name.Should().Be(name);
        }

        [When(@"I request to get all products")]
        public void WhenIRequestToGetAllProducts()
        {
            var result = Task.Run(async () => await Client.GetAsync(ProductEndPoint)).Result;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var teste = result.Content.ReadAsStringAsync().Result;
            var products = ObjectData<List<Product>>(teste);
            products.Count.Should().Be(1);
        }

        [Then(@"Exclude the product with id (.*)")]
        public void ThenExcludeTheProduct(long id)
        {
            var result = Task.Run(async () => await Client.DeleteAsync($"{ProductEndPoint}/{product.Id}")).Result;
            var productResult = ObjectData<Product>(result.Content.ReadAsStringAsync().Result);
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
