using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;
using System;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation
{
    public class DomainValidationTest
    {
        public Faker Faker { get; set; } = new Faker();

        [Fact (DisplayName = nameof(NotNullOk))]
        [Trait("Domain","DomainValidation - Validation")]
        public void NotNullOk()
        {
            var value = Faker.Commerce.ProductName();

            Action action = () => DomainValidation.NotNull(value, "value");

            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(NotNullWhenThrow))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullWhenThrow()
        {
            string value = null;

            Action action = () => DomainValidation.NotNull(value, "FieldName");

            action.Should().Throw<EntityValidationException>();
        }
    }
}
