#include <Python.h>
#include <stdio.h>

int main(void)
{
    Py_Initialize();

    const char* cn = "NewList";
    const char* fn = "script.py";

    FILE* fp = fopen(fn, "rt");
    if (!fp) {
        fprintf(stderr, "File not found: %s\n", fn);
        return -1;
    }

    PyObject* globals = PyDict_New();
    PyObject* locals  = PyDict_New();
    PyDict_SetItemString(globals, "__builtins__", PyEval_GetBuiltins());

    PyObject* res = PyRun_File(fp, fn, Py_file_input, globals, locals);
    fclose(fp);

    if (!res) {
        fprintf(stderr, "Error running file %s\n", fn);
        PyErr_Print();
        Py_DECREF(globals);
        Py_DECREF(locals);
        Py_FinalizeEx();
        return -1;
    }
    Py_DECREF(res);

    PyObject* listClass = PyDict_GetItemString(locals, cn); // borrowed ref
    if (!listClass) {
        fprintf(stderr, "Class not found: %s\n", cn);
        PyErr_Print();
        Py_DECREF(globals);
        Py_DECREF(locals);
        Py_FinalizeEx();
        return -1;
    }

    // создаём список (new reference)
    PyObject* pylist = PyList_New(0);
    PyList_Append(pylist, PyFloat_FromDouble(10));
    PyList_Append(pylist, PyFloat_FromDouble(20));
    PyList_Append(pylist, PyFloat_FromDouble(30));

    // args забирает владение pylist
    PyObject* args = PyTuple_New(1);
    PyTuple_SetItem(args, 0, pylist); // steals reference to pylist
    pylist = NULL;

    PyObject* obj = PyObject_CallObject(listClass, args);
    Py_DECREF(args);

    if (!obj) {
        fprintf(stderr, "Error creating an instance of %s class\n", cn);
        PyErr_Print();
        Py_DECREF(globals);
        Py_DECREF(locals);
        Py_FinalizeEx();
        return -1;
    }

    PyObject* mean = PyObject_CallMethod(obj, "RMS", NULL);
    if (!mean) {
        fprintf(stderr, "RMS() failed\n");
        PyErr_Print();
        Py_DECREF(obj);
        Py_DECREF(globals);
        Py_DECREF(locals);
        Py_FinalizeEx();
        return -1;
    }

    double v = PyFloat_AsDouble(mean);
    if (PyErr_Occurred()) {
        fprintf(stderr, "Result is not a float\n");
        PyErr_Print();
        Py_DECREF(mean);
        Py_DECREF(obj);
        Py_DECREF(globals);
        Py_DECREF(locals);
        Py_FinalizeEx();
        return -1;
    }

    printf("Mean value: %.2f\n", v);

    Py_DECREF(mean);
    Py_DECREF(obj);

    Py_DECREF(globals);
    Py_DECREF(locals);

    Py_FinalizeEx();
    return 0;
}
