using Microsoft.EntityFrameworkCore;
using PinkLoftBooking.Api.Models.Domain;

namespace PinkLoftBooking.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, IConfiguration config, ILogger logger, CancellationToken ct = default)
    {
        await db.Database.EnsureCreatedAsync(ct);

        if (!await db.Resources.AnyAsync(ct))
        {
            db.Resources.AddRange(PinkResources);
            await db.SaveChangesAsync(ct);
            logger.LogInformation("Загружены демо-ресурсы розового лофта.");
        }

        var adminEmail = config["Seed:AdminEmail"] ?? "admin@pinkloft.local";
        var adminPassword = config["Seed:AdminPassword"] ?? "Admin123!";

        if (await db.Users.FirstOrDefaultAsync(u => u.Email == adminEmail, ct) is null)
        {
            db.Users.Add(new AppUser
            {
                Id = Guid.NewGuid(),
                Email = adminEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                Role = UserRole.Admin
            });
            await db.SaveChangesAsync(ct);
            logger.LogInformation("Создан администратор {Email}.", adminEmail);
        }
    }

    private static readonly ResourceEntity[] PinkResources =
    [
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111101"),
            Name = "Бьюти-зона «Сияй»",
            Description =
                "Профессиональный световой стол с LED-лампами, розовый неон, удобное кресло-крыло и подставка для телефона. Идеально для макияжа, съёмок reels и подготовки к выходу. Внутри — бокс с одноразовыми кистями и салфетками."
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111102"),
            Name = "Кладовка с платьями «Fairy Fit»",
            Description =
                "Вечерние платья, пышные юбки, кроп-топы и кардиганы с пайетками. Можно забронировать на вечер (до 23:00) или на выходные. При бронировании выдаётся фирменный розовый плечик для хранения. Дополнительно: примерочная с полным ростовым зеркалом."
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111103"),
            Name = "Фотозона «Pink Light»",
            Description =
                "Кольцевой свет с регулировкой яркости и теплоты, штатив с пультом, розовый фон (выбирай: неон, гирлянды, цветы), зеркальный пол и несколько реквизитов: очки-сердечки, розовый телефон, ободок с ушками. Создана для рилс, фото и настроения."
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111104"),
            Name = "Зал для йоги и пилатеса «Cocoon»",
            Description =
                "Тёплый розовый мат, блоки, ремни, болстеры и пледы. Включается спокойная музыка или звуки природы. Есть диммируемый свет и аромадиффузор с маслом розы или лаванды. В конце занятия — чай с мятой."
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111105"),
            Name = "Кухня для бейкинг-вечеринок «Sprinkle»",
            Description =
                "Просторный стол с розовой скатертью, духовка, миксер, формы для кексов в виде сердечек и звёзд. Есть холодильник для теста и крема. Можно арендовать на 2 или 4 часа. Бонус: розовый фартук и набор топпингов (блёстки, драже, посыпка)."
        },
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111106"),
            Name = "Зона с котокафе «Purr & Relax»",
            Description =
                "Уютная комната с пуфиками, пледами и мягким розовым светом. Внутри живут 3–4 кошки (ласковые, привитые, с именами). Бронирование по часам (от 30 минут до 2 часов). Входит: чашка чая/какао, антисептик для рук и 10 минут обнимашек с котиком в подарок. Кошек нельзя кормить, будить или брать на руки без их согласия (этично 💗)."
        }
    ];
}
