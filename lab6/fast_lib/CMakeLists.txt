cmake_minimum_required(VERSION 3.20)
project(fast_lib C)

set(CMAKE_C_STANDARD 11)
add_library(fast_lib SHARED fast_lib.c)