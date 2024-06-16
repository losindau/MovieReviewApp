using Mapster;
using MovieReview.Application.DTOs;
using MovieReview.Domain.Entities;

namespace MovieReview.Application.Commons.Mapping
{
    public class MovieMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Movie, MovieDto>().TwoWays();
        }
    }
}
