import time

def fib_py(n):
    if n <= 1:
        return n
    return fib_py(n - 1) + fib_py(n - 2)

if __name__ == "__main__":
    n = 35
    start = time.time()
    res = fib_py(n)
    print(f"[Python] fib({n}) = {res}")
    print(f"[Python] Время: {time.time() - start:.4f} сек")