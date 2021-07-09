#import <Foundation/Foundation.h>

@interface MSACExceptionModel : NSObject

@property(nonatomic, copy) NSString *type;
@property(nonatomic, copy) NSString *message;
@property(nonatomic, copy) NSString *stackTrace;

@end
