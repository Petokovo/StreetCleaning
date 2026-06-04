using StreetCleaning.Models;

namespace StreetCleaningTests.Models
{
    public class PagerTests
    {

        private Pager _pager;

        public PagerTests()
        {
            _pager = new Pager();
        }

        [Fact]
        public void CreatePager_DefaultSelectedAmount_Is25()
        {
            Assert.Equal(25, _pager.SelectedAmount);
        }

        [Fact]
        public void CreatePager_DefaultNoMoreResults_IsFalse()
        {
            Assert.Equal("false", _pager.NoMoreResults);
        }

        [Fact]
        public void CreatePager_DefaultSkipAmount_Is0()
        {
            Assert.Equal(0, _pager.SkipAmount);
        }

        [Fact]
        public void SetSelectedAmount_ChangesValue()
        {
            _pager.SelectedAmount = 100;
            Assert.Equal(100, _pager.SelectedAmount);
        }

        [Fact]
        public void SelectedAmount_WhenSetToNegativeValue_ShouldNotBeChanged()
        {
            _pager.SelectedAmount = 25;
            _pager.SelectedAmount = -100;
            Assert.Equal(25, _pager.SelectedAmount);
        }

        [Fact]
        public void SetNoMoreResults_ChangesValue()
        {
            _pager.NoMoreResults = "true";
            Assert.Equal("true", _pager.NoMoreResults);
        }

        [Fact]
        public void SetSkipAmount_ChangesValue()
        {
            _pager.SkipAmount = 100;
            Assert.Equal(100, _pager.SkipAmount);
        }

        [Fact]
        public void SkipAmount_WhenSetToNegativeValue_ShouldNotBeChanged()
        {
            _pager.SkipAmount = 25;
            _pager.SkipAmount = -100;
            Assert.Equal(25, _pager.SkipAmount);
        }
    }
}
