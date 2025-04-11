

```python
def power_func(a, b):
    res = 1
    for i in range(b):
        print(f"Iteration {i+1}: res = {res}")
        res *= a
    return res

def main():
    a = 2
    b = 8
    result = power_func(a, b)
    print(f"Result: {result}")
    print(f"{a} to the power of {b} is {result}")

if __name__ == "__main__":
    main()
```