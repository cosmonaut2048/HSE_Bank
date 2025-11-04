using HSE_Bank.InMemory.Repositories;

namespace HSE_Bank.Application.Facades;

// Реализует паттерн Facade.

/// <summary>
/// Фасад для управления категориями операций.
/// </summary>
public sealed class CategoriesFacade
{
    /// <summary>
    /// Хранилище категорий для сохранения и получения категорий.
    /// </summary>
    private readonly ICategoryStore _cats;
    
    /// <summary>
    /// Фабрика доменных объектов для создания категорий с валидацией.
    /// </summary>
    private readonly Domain.Factories.IFinanceFactory _factory;
    
    /// <summary>
    /// Генератор ID для создания уникальных идентификаторов категорий.
    /// </summary>
    private readonly InMemory.Ids.IIdGenerator _ids;
    
    /// <summary>
    /// Конструктор фасада управления категориями.
    /// </summary>
    /// <param name="cats">Хранилище категорий</param>
    /// <param name="factory">Фабрика для создания доменных объектов</param>
    /// <param name="ids">Генератор ID</param>
    public CategoriesFacade(ICategoryStore cats, Domain.Factories.IFinanceFactory factory, InMemory.Ids.IIdGenerator ids)
    {
        _cats = cats;
        _factory = factory;
        _ids = ids;
    }
    
    /// <summary>
    /// Синхронно создает новую категорию для операций.
    /// </summary>
    /// <param name="name">Название категории</param>
    /// <param name="parent">ID родительской категории для иерархии (необязательно)</param>
    /// <returns>ID созданной категории</returns>
    public Guid Create(string name, Guid? parent = null)
    {
        var c = _factory.CreateCategory(_ids.New(), name, parent);
        _cats.Add(c);
        return c.Id;
    }
    
    /// <summary>
    /// Получает список всех категорий в системе.
    /// </summary>
    /// <returns>Неизменяемый список всех категорий</returns>
    public IReadOnlyList<Domain.Entities.Category> All() => _cats.All();
}