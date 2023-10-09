using FC.Codeflix.Catalog.Domain.Exceptions;
using System;
using System.Linq;
using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };
            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            //Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };
            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
            var datetimeAfter = DateTime.Now;

            //Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.Equal(category.IsActive, isActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = () => new DomainEntity.Category(name!, "Category Description");

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            Action action = () => new DomainEntity.Category("Category Name", null!);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Description should not be empty or null", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThen3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("aa")]
        public void InstantiateErrorWhenNameIsLessThen3Characters(string invalidName)
        {
            Action action = () => new DomainEntity.Category(invalidName, "Category Ok description");

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Contains("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsRatherThen255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsRatherThen255Characters()
        {
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category(invalidName, "Category Ok description");

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Contains("Name should be less or equal 255 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsRatherThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsRatherThen10_000Characters()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category("Category Name", invalidDescription);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Contains("Description should be less or equal 10.000 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, false);

            category.Activate();

            //Assert
            Assert.True(category.IsActive);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, false);

            category.Deactivate();

            //Assert
            Assert.False(category.IsActive);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newValues = new { Name = "new Name", Description = "new Description" };

            category.Update(newValues.Name, newValues.Description);

            Assert.Equal(category.Name, newValues.Name);
            Assert.Equal(category.Description, newValues.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newName = "new Name";
            category.Update(newName, category.Description);
            var currentDescription = category.Description;

            Assert.Equal(category.Name, newName);
            Assert.Equal(category.Description, currentDescription);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");

            Action action = () => category.Update(name!);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThen3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("aa")]
        public void UpdateErrorWhenNameIsLessThen3Characters(string invalidName)
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");

            Action action = () => category.Update(invalidName!);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Contains("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsRatherThen255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsRatherThen255Characters()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            Action action = () => category.Update(invalidName, "Category Ok description");

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Contains("Name should be less or equal 255 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsRatherThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsRatherThen10_000Characters()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

            Action action = () => category.Update("Category Name", invalidDescription);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Contains("Description should be less or equal 10.000 characters long", exception.Message);
        }
    }
}
