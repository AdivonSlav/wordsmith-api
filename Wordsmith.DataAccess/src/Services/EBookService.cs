using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public class EBookService : WriteService<EBookDto, EBook, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>, IEBookService
{
    public EBookService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }
}