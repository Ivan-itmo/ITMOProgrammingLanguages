#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void encrypt(char *text, int key);
void decrypt(char *text, int key);

int main(int argc, char *argv[]) {
    if (argc != 3) {
        printf("Использование: ./caesar <строка> <ключ>\n");
        return 1;
    }

    char *original = argv[1];
    int key = atoi(argv[2]);

    char *text = (char *)malloc(strlen(original) + 1);
    strcpy(text, original);

    printf("Исходный текст: %s\n", text);
    printf("Ключ: %d\n", key);

    encrypt(text, key);
    printf("Зашифрованный текст: %s\n", text);

    decrypt(text, key);
    printf("Расшифрованный текст: %s\n", text);

    free(text);
    return 0;
}

void encrypt(char *text, int key) {
    for (int i = 0; text[i] != '\0'; i++) {
        char c = text[i];
        if (c >= 'a' && c <= 'z') {
            text[i] = 'a' + (c - 'a' + key) % 26;
        } else if (c >= 'A' && c <= 'Z') {
            text[i] = 'A' + (c - 'A' + key) % 26;
        }
    }
}

void decrypt(char *text, int key) {
    for (int i = 0; text[i] != '\0'; i++) {
        char c = text[i];
        if (c >= 'a' && c <= 'z') {
            int shifted = (c - 'a' - key) % 26;
            if (shifted < 0) shifted += 26;
            text[i] = 'a' + shifted;
        } else if (c >= 'A' && c <= 'Z') {
            int shifted = (c - 'A' - key) % 26;
            if (shifted < 0) shifted += 26;
            text[i] = 'A' + shifted;
        }
    }
}