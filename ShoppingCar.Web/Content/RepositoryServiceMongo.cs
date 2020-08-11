using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Awarious.Core.Infrastructure;
using MongoDB.Driver;

namespace Awarious.Core.Repository.Mongo
{
	public class RepositoryServiceMongo<TDocument> : ReadOnlyRepositoryServiceMongo<TDocument>, IRepositoryServiceMongo<TDocument>
		where TDocument : class
	{
		private readonly MongoDbContext _dbContext;
		private readonly IClientSessionHandle _session;
		public IMongoDbContext MongoDbContext { get; }
		public IMongoCollection<TDocument> Documents { get; }

		public RepositoryServiceMongo(IMongoDbContext mongoDbContext, string collectionName = null)
			: base(mongoDbContext, collectionName)
		{
			MongoDbContext = mongoDbContext;
			_dbContext = (MongoDbContext) mongoDbContext;
			Documents = Docs;
		}

		public RepositoryServiceMongo(IMongoDbContext mongoDbContext, IClientSessionHandle session, string collectionName = null)
			: base(mongoDbContext, collectionName)
		{
			_session = session;
			MongoDbContext = mongoDbContext;
			_dbContext = (MongoDbContext)mongoDbContext;
			Documents = Docs;
		}

		public void Add(TDocument document)
		{
			AsyncHelper.RunSync(() => AddAsync(document));
		}

		public async Task AddAsync(TDocument document)
		{
			if (document.GetType().HasInterface<IMongoDocument>())
			{
				if (((IMongoDocument) document).InsertedDate == default(DateTime))
				{
					((IMongoDocument) document).InsertedDate = DateTime.UtcNow;
				}
			}

			if (_session == null)
			{
				await Docs.InsertOneAsync(document);
			}
			else
			{
				int i = 0;
				while (true)
				{
					try
					{
						await Docs.InsertOneAsync(_session, document);
						break;
					}
					catch (MongoCommandException ce)
					{
						if (ce.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
					catch (MongoException e)
					{
						if (e.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
				}
			}
		}

		public void AddAll(List<TDocument> documents)
		{
			AsyncHelper.RunSync(() => AddAllAsync(documents));
		}

		public async Task AddAllAsync(List<TDocument> documents)
		{
			if (typeof(TDocument).HasInterface<IMongoDocument>())
			{
				documents?.ForEach(document =>
				{
					((IMongoDocument)document).InsertedDate = DateTime.UtcNow;
				});
			}

			if (_session == null)
			{
				await Docs.InsertManyAsync(documents);
			}
			else
			{
				int i = 0;
				while (true)
				{
					try
					{
						await Docs.InsertManyAsync(_session, documents);
						break;
					}
					catch (MongoCommandException ce)
					{
						if (ce.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
					catch (MongoException e)
					{
						if (e.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
				}
				
			}
		}

		public void Update(TDocument entity)
		{
			AsyncHelper.RunSync(() => UpdateAsync(entity));
		}

		public async Task UpdateAsync(TDocument document)
		{
			var dynamicPredicateExp = (Expression<Func<TDocument, bool>>)DynamicExpressionParser.ParseLambda(
				typeof(TDocument), typeof(bool),
				$"{IdPropertyName} = @0", typeof(TDocument).GetProperty(IdPropertyName).GetValue(document));

			if (document.GetType().HasInterface<IMongoDocument>())
			{
				((IMongoDocument)document).UpdatedDate = DateTime.UtcNow;
			}

			if (_session == null)
			{
				await Docs.ReplaceOneAsync(dynamicPredicateExp, document);
			}
			else
			{
				int i = 0;
				while (true)
				{
					try
					{
						await Docs.ReplaceOneAsync(_session, dynamicPredicateExp, document);
						break;
					}
					catch (MongoCommandException ce)
					{
						if (ce.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
					catch (MongoException e)
					{
						if (e.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
				}
			}
		}

		public async Task UpdateAsync(Expression<Func<TDocument, bool>> predicate, UpdateDefinition<TDocument> updateDefinition)
		{
			if (typeof(TDocument).HasInterface<IMongoDocument>())
			{
				updateDefinition.Set(x => ((IMongoDocument)x).UpdatedDate, DateTime.UtcNow);
			}

			if (_session == null)
			{
				await Docs.UpdateManyAsync(predicate, updateDefinition);
			}
			else
			{
				int i = 0;
				while (true)
				{
					try
					{
						await Docs.UpdateManyAsync(_session, predicate, updateDefinition);
						break;
					}
					catch (MongoCommandException ce)
					{
						if (ce.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
					catch (MongoException e)
					{
						if (e.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
				}
			}
		}

		public void UpdateAll(IEnumerable<TDocument> documents)
		{
			AsyncHelper.RunSync(() => UpdateAllAsync(documents));
		}

		public async Task UpdateAllAsync(IEnumerable<TDocument> documents)
		{
			foreach (var document in documents)
			{
				await UpdateAsync(document);
			}
		}

		public void Update<TKey>(TKey id, UpdateDefinition<TDocument> updateDefinition)
		{
			AsyncHelper.RunSync(() => UpdateAsync(id, updateDefinition));
		}

		public async Task UpdateAsync<TKey>(TKey id, UpdateDefinition<TDocument> updateDefinition)
		{
			var dynamicPredicateExp = (Expression<Func<TDocument, bool>>)DynamicExpressionParser.ParseLambda(
				typeof(TDocument), typeof(bool),
				$"{IdPropertyName} = @0", id);

			await UpdateAsync(dynamicPredicateExp, updateDefinition);
		}
		
		public void Remove(Expression<Func<TDocument, bool>> predicate)
		{
			AsyncHelper.RunSync(() => RemoveAsync(predicate));
		}

		public async Task RemoveAsync(Expression<Func<TDocument, bool>> predicate)
		{
			if (_session == null)
			{
				await Docs.DeleteManyAsync(predicate);
			}
			else
			{
				int i = 0;
				while (true)
				{
					try
					{
						await Docs.DeleteManyAsync(_session, predicate);
						break;
					}
					catch (MongoCommandException ce)
					{
						if (ce.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
					catch (MongoException e)
					{
						if (e.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
				}
			}
		}

		public TDocument Remove<TKey>(TKey id)
		{
			return AsyncHelper.RunSync(() => RemoveAsync(id));
		}

		public async Task<TDocument> RemoveAsync<TKey>(TKey id)
		{
			var dynamicPredicateExp = (Expression<Func<TDocument, bool>>)DynamicExpressionParser.ParseLambda(
				typeof(TDocument), typeof(bool),
				$"{IdPropertyName} = @0", id);

			var document = await FindFirstAsync(dynamicPredicateExp);

			if (_session == null)
			{
				await Docs.DeleteOneAsync(dynamicPredicateExp);
			}
			else
			{
				int i = 0;
				while (true)
				{
					try
					{
						await Docs.DeleteOneAsync(_session, dynamicPredicateExp);
						break;
					}
					catch (MongoCommandException ce)
					{
						if (ce.HasErrorLabel("TransientTransactionError") && i <= 100 && ce.Code != 251)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
					catch (MongoException e)
					{
						if (e.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
				}
			}

			return document;
		}

		public void Remove(TDocument document)
		{
			AsyncHelper.RunSync(() => RemoveAsync(document));
		}

		public async Task RemoveAsync(TDocument document)
		{
			var dynamicPredicateExp = (Expression<Func<TDocument, bool>>)DynamicExpressionParser.ParseLambda(
				typeof(TDocument), typeof(bool),
				$"{IdPropertyName} = @0", typeof(TDocument).GetProperty(IdPropertyName)?.GetValue(document));

			if (_session == null)
			{
				await Docs.DeleteOneAsync(dynamicPredicateExp);
			}
			else
			{
				int i = 0;
				while (true)
				{
					try
					{
						await Docs.DeleteOneAsync(_session, dynamicPredicateExp);
						break;
					}
					catch (MongoCommandException ce)
					{
						if (ce.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
					catch (MongoException e)
					{
						if (e.HasErrorLabel("TransientTransactionError") && i <= 100)
						{
							await _dbContext.StartNewTransaction(_session);
							i++;
							continue;
						}

						await _dbContext.AbortExistingTransactionAsync(_session);
						throw;
					}
				}
			}
		}

	}
}
