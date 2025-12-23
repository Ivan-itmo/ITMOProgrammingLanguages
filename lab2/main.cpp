#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

typedef struct {
    int x;
    int y;
} Point;

typedef enum {
    WTYPE,
    ATYPE,
    MTYPE
} UnitType;

typedef struct {
    char name[50];
    int health;
    int maxHealth;
    Point position;
    UnitType type;
    union {
        struct {
            int damage;
        } warrior;
        struct {
            int damage;
            int range;
        } archer;
        struct {
            int mag_damage;
            int range;
        } mage;
    } stats;
} GameUnit;

void MoveUnit(GameUnit* unit, Point shift) {
    unit->position.x += shift.x;
    unit->position.y += shift.y;
}

void RandomMoveUnits(GameUnit* units, int count, Point max_shift) {
    for (int i = 0; i < count; ++i) {
        int dx = (rand() % (2 * max_shift.x + 1)) - max_shift.x;
        int dy = (rand() % (2 * max_shift.y + 1)) - max_shift.y;
        Point random_shift = {dx, dy};
        MoveUnit(&units[i], random_shift);
    }
}

void PrintUnits(GameUnit* units, int count) {
    for (int i = 0; i < count; ++i) {
        GameUnit* u = &units[i];
        printf("Юнит [%d]:\n", i);
        printf("  Имя: %s\n", u->name);
        printf("  Здоровье: %d / %d\n", u->health, u->maxHealth);
        printf("  Позиция: (%d, %d)\n", u->position.x, u->position.y);

        switch (u->type) {
            case WTYPE:
                printf("  Тип: Воин\n");
                printf("  Урон: %d\n", u->stats.warrior.damage);
                break;
            case ATYPE:
                printf("  Тип: Лучник\n");
                printf("  Урон: %d\n", u->stats.archer.damage);
                printf("  Дальность: %d\n", u->stats.archer.range);
                break;
            case MTYPE:
                printf("  Тип: Маг\n");
                printf("  Магический урон: %d\n", u->stats.mage.mag_damage);
                printf("  Дальность: %d\n", u->stats.mage.range);
                break;
            default:
                printf("  Тип: Неизвестно\n");
                break;
        }
        printf("\n");
    }
}

void Attack(GameUnit* attacker, GameUnit* target) {
    int damage = 0;
    switch (attacker->type) {
        case WTYPE:
            damage = attacker->stats.warrior.damage;
            break;
        case ATYPE:
            damage = attacker->stats.archer.damage;
            break;
        case MTYPE:
            damage = attacker->stats.mage.mag_damage;
            break;
        default:
            damage = 0;
    }

    target->health -= damage;
    if (target->health < 0) target->health = 0;

    printf("%s атакует %s и наносит %d урона. Здоровье цели: %d/%d\n",
           attacker->name, target->name, damage, target->health, target->maxHealth);
}

int main() {
    const int COUNT = 10;
    GameUnit* army = (GameUnit*)malloc(COUNT * sizeof(GameUnit));
    if (army == NULL) {
        printf("Ошибка: не удалось выделить память под армию!\n");
        return 1;
    }

    army[0] = (GameUnit){"Воин1", 100, 100, {0, 0}, WTYPE, .stats.warrior = {30}};
    army[1] = (GameUnit){"Воин2", 100, 100, {0, 0}, WTYPE, .stats.warrior = {35}};
    army[2] = (GameUnit){"Воин3", 100, 100, {0, 0}, WTYPE, .stats.warrior = {25}};
    army[3] = (GameUnit){"Лучник1", 70, 70, {0, 0}, ATYPE, .stats.archer = {20, 50}};
    army[4] = (GameUnit){"Лучник2", 75, 75, {0, 0}, ATYPE, .stats.archer = {22, 55}};
    army[5] = (GameUnit){"Лучник3", 65, 65, {0, 0}, ATYPE, .stats.archer = {18, 45}};
    army[6] = (GameUnit){"Маг1", 50, 50, {0, 0}, MTYPE, .stats.mage = {40, 60}};
    army[7] = (GameUnit){"Маг2", 55, 55, {0, 0}, MTYPE, .stats.mage = {45, 65}};
    army[8] = (GameUnit){"Маг3", 45, 45, {0, 0}, MTYPE, .stats.mage = {35, 55}};
    army[9] = (GameUnit){"Маг4", 55, 55, {0, 0}, MTYPE, .stats.mage = {30, 45}};

    printf("Исходное состояние\n");
    PrintUnits(army, COUNT);

    printf("\nСлучайное перемещение\n");
    RandomMoveUnits(army, COUNT, (Point){10, 10});
    PrintUnits(army, COUNT);

    printf("\nАТАКИ\n");
    Attack(&army[0], &army[2]);
    Attack(&army[1], &army[4]);

    printf("\nСостояние после атак\n");
    PrintUnits(army, COUNT);

    free(army);
    return 0;
}