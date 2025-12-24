import ctypes
import time
import os

lib_name = "fast_lib.dll" if os.name == "nt" else "fast_lib.so"
lib_path = os.path.join(os.path.dirname(__file__), lib_name)

c_lib = ctypes.CDLL(lib_path)
c_lib.fib_c.argtypes = [ctypes.c_int]
c_lib.fib_c.restype = ctypes.c_longlong

c_lib.fib_c_iterative.argtypes = [ctypes.c_int]
c_lib.fib_c_iterative.restype = ctypes.c_longlong

class Point(ctypes.Structure):
    _fields_ = [("x", ctypes.c_int), ("y", ctypes.c_int)]

c_lib.process_point.argtypes = [ctypes.POINTER(Point)]
c_lib.process_point.restype = None

# Тест
if __name__ == "__main__":
    n = 35
    def fib_py(n):
        if n <= 1: return n
        return fib_py(n - 1) + fib_py(n - 2)

    start = time.time()
    res_py = fib_py(n)
    py_time = time.time() - start

    start = time.time()
    res_c = c_lib.fib_c(n)
    c_time = time.time() - start

    start = time.time()
    res_iter = c_lib.fib_c_iterative(n)
    c_iter_time = time.time() - start

    print(f"[Python] fib({n}) = {res_py}, время: {py_time:.4f} сек")
    print(f"[C rec]  fib({n}) = {res_c}, время: {c_time:.4f} сек")
    print(f"[C iter] fib({n}) = {res_iter}, время: {c_iter_time:.4f} сек")

    if c_time > 0:
        print(f"\nC (рекурсия) быстрее Python в {py_time / c_time:.0f} раз")

    p = Point(4, 9)
    print(f"\n[Python] до: ({p.x}, {p.y})")
    c_lib.process_point(ctypes.byref(p))
    print(f"[Python] после: ({p.x}, {p.y})")