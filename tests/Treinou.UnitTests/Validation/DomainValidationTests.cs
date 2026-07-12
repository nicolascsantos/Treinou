using Treinou.Domain.Exceptions;
using Treinou.Domain.Validation;

namespace Treinou.UnitTests.Validation
{
    public class DomainValidationTests
    {
        [Fact(DisplayName = nameof(NotNullThrowsWhenNull))]
        [Trait("Domain", "DomainValidation")]
        public void NotNullThrowsWhenNull()
        {
            object? target = null;

            var action = () => DomainValidation.NotNull(target, "FieldName");

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(NotNullDoesNotThrowWhenValue))]
        [Trait("Domain", "DomainValidation")]
        public void NotNullDoesNotThrowWhenValue()
        {
            object target = new();

            var exception = Record.Exception(() => DomainValidation.NotNull(target, "FieldName"));

            Assert.Null(exception);
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowsWhenNullOrEmptyOrWhitespace))]
        [Trait("Domain", "DomainValidation")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NotNullOrEmptyThrowsWhenNullOrEmptyOrWhitespace(string? target)
        {
            var action = () => DomainValidation.NotNullOrEmpty(target, "FieldName");

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(NotNullOrEmptyDoesNotThrowWhenValidString))]
        [Trait("Domain", "DomainValidation")]
        public void NotNullOrEmptyDoesNotThrowWhenValidString()
        {
            var exception = Record.Exception(() => DomainValidation.NotNullOrEmpty("valid value", "FieldName"));

            Assert.Null(exception);
        }

        [Fact(DisplayName = nameof(MinLengthThrowsWhenTooShort))]
        [Trait("Domain", "DomainValidation")]
        public void MinLengthThrowsWhenTooShort()
        {
            var action = () => DomainValidation.MinLength("ab", "FieldName", 3);

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(MinLengthDoesNotThrowAtExactMinimum))]
        [Trait("Domain", "DomainValidation")]
        public void MinLengthDoesNotThrowAtExactMinimum()
        {
            var exception = Record.Exception(() => DomainValidation.MinLength("abc", "FieldName", 3));

            Assert.Null(exception);
        }

        [Fact(DisplayName = nameof(MaxLengthThrowsWhenTooLong))]
        [Trait("Domain", "DomainValidation")]
        public void MaxLengthThrowsWhenTooLong()
        {
            var action = () => DomainValidation.MaxLength("abcd", "FieldName", 3);

            Assert.Throws<EntityValidationException>(action);
        }

        [Fact(DisplayName = nameof(MaxLengthDoesNotThrowAtExactMaximum))]
        [Trait("Domain", "DomainValidation")]
        public void MaxLengthDoesNotThrowAtExactMaximum()
        {
            var exception = Record.Exception(() => DomainValidation.MaxLength("abc", "FieldName", 3));

            Assert.Null(exception);
        }
    }
}
