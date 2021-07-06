#import <Foundation/Foundation.h>

@interface MSACException : NSObject

@property(nonatomic, copy) NSString *type;
@property(nonatomic, copy) NSString *message;
@property(nonatomic, copy) NSString *stackTrace;

@end
